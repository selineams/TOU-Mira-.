using System.Text;
using AmongUs.GameOptions;
using HarmonyLib;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules;
using TownOfUs.Modules.Components;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Patches.Stubs;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Neutral;

public sealed class DoomsayerRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    public string RoleName => "末日预言者";
    public string RoleDescription => "猜中他人身份即可获胜！";
    public string RoleLongDescription => $"通过猜中 {(int)OptionGroupSingleton<DoomsayerOptions>.Instance.DoomsayerGuessesToWin} 名玩家的身份获胜";
    public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<VigilanteRole>());
    public Color RoleColor => TownOfUsColors.Doomsayer;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    public DoomableType DoomHintType => DoomableType.Insight;
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.QuestionSound,
        Icon = TouRoleIcons.Doomsayer,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>(),
    };

    public int NumberOfGuesses { get; set; }
    public int IncorrectGuesses { get; set; }
    public bool AllGuessesCorrect { get; set; }
    [HideFromIl2Cpp]
    public List<PlayerControl> AllVictims { get; } = [];

    private MeetingMenu meetingMenu;
    public bool MetWinCon => AllGuessesCorrect;

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);

        if (Player.AmOwner)
        {
            meetingMenu = new MeetingMenu(
                this,
                ClickGuess,
                MeetingAbilityType.Click,
                TouAssets.Guess,
                null!,
                IsExempt);
        }
    }

    public override void OnMeetingStart()
    {
        RoleStubs.RoleBehaviourOnMeetingStart(this);

        if (Player.AmOwner)
        {
            meetingMenu.GenButtons(MeetingHud.Instance, Player.AmOwner && !Player.HasDied() && !Player.HasModifier<JailedModifier>());

            if (OptionGroupSingleton<DoomsayerOptions>.Instance.DoomsayerGuessAllAtOnce) NumberOfGuesses = 0;
            IncorrectGuesses = 0;
            AllVictims.Clear();
            AllGuessesCorrect = false;
        }

        GenerateReport();
    }

    public override void OnVotingComplete()
    {
        RoleStubs.RoleBehaviourOnVotingComplete(this);

        if (Player.AmOwner)
        {
            meetingMenu.HideButtons();
        }
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        if (Player.AmOwner)
        {
            meetingMenu?.Dispose();
            meetingMenu = null!;
        }
    }

    private void GenerateReport()
    {
        Logger<TownOfUsPlugin>.Info($"生成末日报告");

        var reportBuilder = new StringBuilder();

        if (Player == null) return;
        if (!Player.AmOwner) return;

        foreach (var player in GameData.Instance.AllPlayers.ToArray().Where(x => !x.Object.HasDied() && x.Object.HasModifier<DoomsayerObservedModifier>()))
        {
            var role = player.Object.Data.Role;
            var doomableRole = role as IDoomable;
            var hintType = DoomableType.Default;
            var cachedMod = player.Object.GetModifiers<BaseModifier>().FirstOrDefault(x => x is ICachedRole) as ICachedRole;
            if (cachedMod != null)
            {
                role = cachedMod.CachedRole;
                doomableRole = role as IDoomable;
            }
            var unguessableMod = player.Object.GetModifiers<BaseModifier>().FirstOrDefault(x => x is IUnguessable) as IUnguessable;
            if (unguessableMod != null)
            {
                role = unguessableMod.AppearAs;
                doomableRole = role as IDoomable;
            }

            if (doomableRole != null)
            {
                hintType = doomableRole.DoomHintType;
            }

            if (hintType == DoomableType.Default) continue;

            switch (hintType)
            {
                case DoomableType.Perception:
                    reportBuilder.AppendLine(TownOfUsPlugin.Culture, $"你观察到{player.PlayerName}对现实有不同的感知\n");
                    break;
                case DoomableType.Insight:
                    reportBuilder.AppendLine(TownOfUsPlugin.Culture, $"你观察到{player.PlayerName}对隐私信息有洞察力\n");
                    break;
                case DoomableType.Death:
                    reportBuilder.AppendLine(TownOfUsPlugin.Culture, $"你观察到{player.PlayerName}对尸体有异常的执着\n");
                    break;
                case DoomableType.Hunter:
                    reportBuilder.AppendLine(TownOfUsPlugin.Culture, $"你观察到{player.PlayerName}擅长追猎猎物\n");
                    break;
                case DoomableType.Fearmonger:
                    reportBuilder.AppendLine(TownOfUsPlugin.Culture, $"你观察到{player.PlayerName}在队伍中散播恐惧\n");
                    break;
                case DoomableType.Protective:
                    reportBuilder.AppendLine(TownOfUsPlugin.Culture, $"你观察到{player.PlayerName}会隐藏自己或保护他人\n");
                    break;
                case DoomableType.Trickster:
                    reportBuilder.AppendLine(TownOfUsPlugin.Culture, $"你观察到{player.PlayerName}暗藏诡计\n");
                    break;
                case DoomableType.Relentless:
                    reportBuilder.AppendLine(TownOfUsPlugin.Culture, $"你观察到{player.PlayerName}能够发动无情的攻击\n");
                    break;
            }
            var roles = MiscUtils.AllRoles.Where(x => x is IDoomable doomRole && doomRole.DoomHintType == hintType && x is not IUnguessable).OrderBy(x => x.NiceName).ToList();
            var lastRole = roles[roles.Count - 1];
            roles.Remove(roles[roles.Count - 1]);

            if (roles.Count != 0)
            {
                reportBuilder.Append(TownOfUsPlugin.Culture, $"(");
                foreach (var role2 in roles)
                {
                    reportBuilder.Append(TownOfUsPlugin.Culture, $"{role2.NiceName}, ");
                }
                reportBuilder = reportBuilder.Remove(reportBuilder.Length - 2, 2);
                reportBuilder.Append(TownOfUsPlugin.Culture, $" or {lastRole.NiceName})");
            }

            player.Object.RemoveModifier<DoomsayerObservedModifier>();
        }

        var report = reportBuilder.ToString();

        if (HudManager.Instance && report.Length > 0)
        {
            var title = $"<color=#{TownOfUsColors.Doomsayer.ToHtmlStringRGBA()}>末日报告</color>";
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
        return AllGuessesCorrect;
    }

    public bool WinConditionMet()
    {
        if (Player.HasDied()) return false;
        if (OptionGroupSingleton<DoomsayerOptions>.Instance.DoomWin is not DoomWinOptions.EndsGame) return false;

        return AllGuessesCorrect;
    }

    public void ClickGuess(PlayerVoteArea voteArea, MeetingHud meetingHud)
    {
        if (meetingHud.state == MeetingHud.VoteStates.Discussion)
        {
            return;
        }

        var player = GameData.Instance.GetPlayerById(voteArea.TargetPlayerId).Object;

        var shapeMenu = GuesserMenu.Create();
        shapeMenu.Begin(IsRoleValid, ClickRoleHandle);

        void ClickRoleHandle(RoleBehaviour role)
        {
            var realRole = player.Data.Role;

            var cachedMod = player.GetModifiers<BaseModifier>().FirstOrDefault(x => x is ICachedRole) as ICachedRole;
            if (cachedMod != null)
            {
                realRole = cachedMod.CachedRole;
            }

            var pickVictim = role.Role == realRole.Role;
            var victim = pickVictim ? player : Player;

            ClickHandler(victim, voteArea.TargetPlayerId);
        }

        void ClickHandler(PlayerControl victim, byte targetId)
        {
            var opts = OptionGroupSingleton<DoomsayerOptions>.Instance;

            if (opts.DoomsayerGuessAllAtOnce) NumberOfGuesses++;
            meetingMenu?.HideSingle(targetId);

            var playersAlive = PlayerControl.AllPlayerControls.ToArray().Count(x => !x.HasDied() && !x.IsJailed() && x != Player);

            if (victim == Player)
            {
                IncorrectGuesses++;
                if (!opts.DoomsayerGuessAllAtOnce)
                {
                    Coroutines.Start(MiscUtils.CoFlash(Color.red));
                    meetingMenu?.HideButtons();
                    shapeMenu.Close();
                    return;
                }
            }
            else if (!opts.DoomsayerGuessAllAtOnce)
            {
                Coroutines.Start(MiscUtils.CoFlash(Color.green));
                NumberOfGuesses++;
            }
            else
            {
                AllVictims.Add(victim);
            }
            if (((NumberOfGuesses < 2 && playersAlive < 3) || (NumberOfGuesses < (int)opts.DoomsayerGuessesToWin && playersAlive > 2)) && opts.DoomsayerGuessAllAtOnce)
            {
                shapeMenu.Close();
                return;
            }

            if (IncorrectGuesses > 0 && opts.DoomsayerGuessAllAtOnce)
            {
                var text = (NumberOfGuesses - AllVictims.Count) == 1 ? $"<b>只有一次猜测错误！</b>" : $"<b>{NumberOfGuesses - AllVictims.Count}次猜测错误。</b>";
                var notif1 = Helpers.CreateAndShowNotification(
                    text, Color.white, spr: TouRoleIcons.Doomsayer.LoadAsset());

                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);

                Coroutines.Start(MiscUtils.CoFlash(Color.red));
            }
            else if (opts.DoomsayerGuessAllAtOnce)
            {
                if (opts.DoomsayerKillOnlyLast) Player.RpcCustomMurder(victim, createDeadBody: false, teleportMurderer: false);
                else AllVictims.Do(victim => Player.RpcCustomMurder(victim, createDeadBody: false, teleportMurderer: false));
            }
            else
            {
                // no incorrect guesses so this should be the target not the Doomsayer
                Player.RpcCustomMurder(victim, createDeadBody: false, teleportMurderer: false);
            }

            if (opts.DoomsayerGuessAllAtOnce || NumberOfGuesses == (int)opts.DoomsayerGuessesToWin) meetingMenu?.HideButtons();

            shapeMenu.Close();
        }
    }

    public bool IsExempt(PlayerVoteArea voteArea)
    {
        return voteArea.TargetPlayerId == Player.PlayerId ||
        Player.Data.IsDead || voteArea.AmDead ||
        voteArea.GetPlayer()?.HasModifier<JailedModifier>() == true ||
        voteArea.GetPlayer()?.Data.Role is MayorRole mayor && mayor.Revealed ||
        Player.IsLover() && voteArea.GetPlayer()?.IsLover() == true;
    }

    private static bool IsRoleValid(RoleBehaviour role)
    {
        var unguessableRole = role as IUnguessable;
        if (role.IsDead || role is IGhostRole || (unguessableRole != null && !unguessableRole.IsGuessable))
        {
            return false;
        }

        return true;
    }

    [MethodRpc((uint)TownOfUsRpc.DoomsayerWin, SendImmediately = true)]
    public static void RpcDoomsayerWin(PlayerControl player)
    {
        if (player.Data.Role is not DoomsayerRole)
        {
            Logger<TownOfUsPlugin>.Error("RpcDoomsayerWin - Invalid Doomsayer");
            return;
        }

        var doom = player.GetRole<DoomsayerRole>();
        doom!.AllGuessesCorrect = true;

        if (GameHistory.PlayerStats.TryGetValue(player.PlayerId, out var stats))
        {
            stats.CorrectAssassinKills++;
        }
    }

    public string GetAdvancedDescription()
    {
        return $"末日预言家是一名中立邪恶型角色，通过猜中{(int)OptionGroupSingleton<DoomsayerOptions>.Instance.DoomsayerGuessesToWin}名玩家的身份获胜。" + (OptionGroupSingleton<DoomsayerOptions>.Instance.CantObserve ? string.Empty : "可在会议前观察玩家，获得其身份线索。") + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("洞察",
            "洞察一名玩家，在下次会议获得其身份线索。",
            TouNeutAssets.Observe)
    ];
}
