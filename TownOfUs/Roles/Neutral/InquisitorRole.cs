using System.Collections;
using System.Globalization;
using System.Text;
using AmongUs.GameOptions;
using HarmonyLib;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Patches.Stubs;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Neutral;

public sealed class InquisitorRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, IAssignableTargets, ICrewVariant
{
    public string RoleName => "审判官";
    public string RoleDescription => "消灭异端！";
    public string RoleLongDescription => "消灭你的异端目标或让他们被杀。所有异端死亡后你将获胜并离开游戏。";
    public Color RoleColor => TownOfUsColors.Inquisitor;
    public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<OracleRole>());
    public bool CanVanquish { get; set; } = true;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    public DoomableType DoomHintType => DoomableType.Hunter;
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.ToppatIntroSound,
        Icon = TouRoleIcons.Inquisitor,
        MaxRoleCount = 1,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>(),
    };
    public int Priority { get; set; } = 5;

    [HideFromIl2Cpp]
    public List<PlayerControl> Targets { get; set; } = [];
    [HideFromIl2Cpp]
    public List<RoleBehaviour> TargetRoles { get; set; } = [];
    public bool TargetsDead { get; set; }
    public bool MetWinCon => TargetsDead;

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);
        CanVanquish = true;

        // if Inuquisitor was revived
        if (Targets.Count == 0)
        {
            Targets = ModifierUtils.GetPlayersWithModifier<InquisitorHereticModifier>().ToList();
            TargetRoles = ModifierUtils.GetActiveModifiers<InquisitorHereticModifier>().Select([HideFromIl2Cpp] (x) => x.TargetRole).OrderBy([HideFromIl2Cpp] (x) => x.NiceName).ToList();
        }
        if (TutorialManager.InstanceExists && Targets.Count == 0 && Player.AmOwner && Player.IsHost() && AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started)
        {
            Coroutines.Start(SetTutorialTargets(this));
        }
    }
    private static IEnumerator SetTutorialTargets(InquisitorRole inquis)
    {
        yield return new WaitForSeconds(0.01f);
        inquis.AssignTargets();
    }
    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        if (TutorialManager.InstanceExists && Player.AmOwner)
        {
            var players = ModifierUtils.GetPlayersWithModifier<InquisitorHereticModifier>().ToList();
            players.Do(x => x.RpcRemoveModifier<InquisitorHereticModifier>());
        }
    }
    public override void OnMeetingStart()
    {
        RoleStubs.RoleBehaviourOnMeetingStart(this);

        if (Player.AmOwner)
        {
            GenerateReport();
        }
    }
    private void GenerateReport()
    {
        Logger<TownOfUsPlugin>.Info($"Generating Inquisitor report");

        var reportBuilder = new StringBuilder();

        if (Player == null) return;
        if (!Player.AmOwner) return;

        foreach (var player in GameData.Instance.AllPlayers.ToArray().Where(x => !x.Object.HasDied() && x.Object.HasModifier<InquisitorInquiredModifier>()))
        {
            if (player.Object.HasModifier<InquisitorHereticModifier>())
            {
                reportBuilder.AppendLine(TownOfUsPlugin.Culture, $"你的调查显示{player.PlayerName}是异端！\n");
                var roles = TargetRoles;
                var lastRole = roles[roles.Count - 1];

                if (roles.Count != 0)
                {
                    reportBuilder.Append(TownOfUsPlugin.Culture, $"(");
                    foreach (var role2 in roles)
                    {
                        reportBuilder.Append(TownOfUsPlugin.Culture, $"{role2.NiceName}, ");
                    }
                    reportBuilder = reportBuilder.Remove(reportBuilder.Length - 2, 2);
                    reportBuilder.Append(TownOfUsPlugin.Culture, $"或{lastRole.NiceName})");
                }
            }
            else
                reportBuilder.AppendLine(TownOfUsPlugin.Culture, $"你的调查显示{player.PlayerName}不是异端！");

            player.Object.RemoveModifier<InquisitorInquiredModifier>();
        }

        var report = reportBuilder.ToString();

        if (HudManager.Instance && report.Length > 0)
        {
            var title = $"<color=#{TownOfUsColors.Inquisitor.ToHtmlStringRGBA()}>审判官报告</color>";
            MiscUtils.AddFakeChat(Player.Data, title, report, false, true);
        }
    }

    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }
        Console console = usable.TryCast<Console>()!;
        return (console == null) || console.AllowImpostor;
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return TargetsDead || WinConditionMet();
    }
    public bool WinConditionMet()
    {
        if (Player.HasDied()) return false;
        if (!OptionGroupSingleton<InquisitorOptions>.Instance.StallGame) return false;
        if (!TargetsDead) return false;

        var result = Helpers.GetAlivePlayers().Count <= 2 && MiscUtils.KillersAliveCount == 1;
        return result;
    }

    public void CheckTargetDeath(PlayerControl exiled)
    {
        if (Player.HasDied()) return;
        if (Targets.Count == 0) return;

        if (Targets.All(x => x.HasDied()))
        {
            // Logger<TownOfUsPlugin>.Error($"CheckTargetEjection - exiled: {exiled.Data.PlayerName}");
            RpcInquisitorWin(Player);
        }
    }

    public void AssignTargets()
    {
        var inquis = PlayerControl.AllPlayerControls.ToArray()
            .FirstOrDefault(x => x.IsRole<InquisitorRole>() && !x.HasDied());

        if (inquis == null)
        {
            Logger<TownOfUsPlugin>.Error("Inquisitor not found.");
            return;
        }

        var required = (int)OptionGroupSingleton<InquisitorOptions>.Instance.AmountOfHeretics;
        var players = PlayerControl.AllPlayerControls.ToArray().Where(x => x.Data.Role is not InquisitorRole).ToList();
        // Logger<TownOfUsPlugin>.Warning($"Players in heretic list possible: {players.Count}");
        players.Shuffle();
        players.Shuffle();
        players.Shuffle();

        var evil = players.Any(x => x.IsNeutral() || x.IsImpostor()) ? players.FirstOrDefault(x => x.IsNeutral() || x.IsImpostor()) : players.Random();
        players.Remove(evil);
        players.Shuffle();

        var crew = players.Any(x => x.IsCrewmate()) ? players.FirstOrDefault(x => x.IsCrewmate()) : players.Random();
        players.Remove(crew);
        players.Shuffle();

        var random = players.Random();
        players.Remove(random);
        players.Shuffle();

        List<PlayerControl> filtered = [];

        if (evil != null) filtered.Add(evil);
        if (crew != null) filtered.Add(crew);
        if (random != null) filtered.Add(random);

        var other = players.Random();
        if (required is 4 or 5 && players.Count >= 1 && other != null)
        {
            filtered.Add(other);
            players.Remove(other);
        }
        players.Shuffle();
        other = players.Random();
        if (required is 5 && players.Count >= 1 && other != null) filtered.Add(other);

        if (filtered.Count > 0)
        {
            filtered = filtered.OrderBy(x => x.Data.Role.NiceName).ToList();
            foreach (var player in filtered)
            {
                RpcAddInquisTarget(inquis, player);
            }
        }
    }

    [MethodRpc((uint)TownOfUsRpc.AddInquisTarget, SendImmediately = true)]
    public static void RpcAddInquisTarget(PlayerControl player, PlayerControl target)
    {
        if (player.Data.Role is not InquisitorRole)
        {
            Logger<TownOfUsPlugin>.Error("RpcAddInquisTarget - Invalid Inquisitor");
            return;
        }

        if (target == null) return;

        var role = player.GetRole<InquisitorRole>();

        if (role == null) return;

        role.Targets.Add(target);
        role.TargetRoles.Add(target.Data.Role);
        target.AddModifier<InquisitorHereticModifier>();
    }

    [MethodRpc((uint)TownOfUsRpc.InquisitorWin, SendImmediately = true)]
    public static void RpcInquisitorWin(PlayerControl player)
    {
        if (player.Data.Role is not InquisitorRole)
        {
            Logger<TownOfUsPlugin>.Error("RpcInquisitorWin - Invalid Inquisitor");
            return;
        }

        var exe = player.GetRole<InquisitorRole>();
        exe!.TargetsDead = true;
    }
    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);
        stringB.AppendLine(CultureInfo.InvariantCulture, $"<b>你的异端目标身份：</b>");
        foreach (var role in TargetRoles)
        {
            var newText = $"<b><size=80%>{role.TeamColor.ToTextColor()}{role.NiceName}</size></b>";
            stringB.AppendLine(CultureInfo.InvariantCulture, $"{newText}");
        }

        return stringB;
    }

    public string GetAdvancedDescription()
    {
        return $"审判官是一名中立邪恶型角色，只要所有异端目标死亡即可获胜。你只知道他们的身份，需要自己找出这些玩家（对死者以<color=#D94291>$</color>标记），并想办法让他们被杀。" + MiscUtils.AppendOptionsText(GetType());
    }
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("调查",
            "调查一名玩家，在会议中得知其是否为你的目标。",
            TouNeutAssets.InquireSprite),
        new("审判",
            "审判一名玩家。如果对方是异端，你会被告知并可继续审判；如果不是异端，你将失去审判能力。",
            TouNeutAssets.InquisKillSprite)
    ];
}
