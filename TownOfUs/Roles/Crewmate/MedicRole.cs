using System.Globalization;
using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class MedicRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "法医";
    public string RoleDescription => "为船员加护盾进行保护";
    public string RoleLongDescription => "用护盾保护一名船员";
    public Color RoleColor => TownOfUsColors.Medic;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateProtective;
    public DoomableType DoomHintType => DoomableType.Protective;
    public override bool IsAffectedByComms => false;
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Scientist),
        Icon = TouRoleIcons.Medic,
    };

    public PlayerControl? Shielded { get; set; }

    private MeetingMenu meetingMenu;

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);

        if (Player.AmOwner)
        {
            meetingMenu = new MeetingMenu(
                this,
                (PlayerVoteArea _, MeetingHud _) => { },
                MeetingAbilityType.Click,
                TouAssets.LighterSprite,
                null!,
                (PlayerVoteArea voteArea) => { return Player.Data.IsDead || voteArea!.AmDead; },
                hoverColor: Color.white)
                {
                    Position = new Vector3(1.1f, -0.18f, -3f),
                };
        }
    }

    public override void OnMeetingStart()
    {
        RoleStubs.RoleBehaviourOnMeetingStart(this);

        if (Player.AmOwner)
        {
            meetingMenu.GenButtons(MeetingHud.Instance, Player.AmOwner && !Player.HasDied() && !Player.HasModifier<JailedModifier>());

            foreach (var button in meetingMenu.Buttons)
            {
                if (button.Value == null) continue;

                button.Value.transform.localScale *= 0.8f;

                var player = MiscUtils.PlayerById(button.Key);

                if (player == null ) continue;

                var colorType = GetColorTypeForPlayer(player);

                var renderer = button.Value.GetComponent<SpriteRenderer>();

                if (renderer == null) continue;

                renderer.sprite = colorType switch
                {
                    "浅色" => TouAssets.LighterSprite.LoadAsset(),
                    _ => TouAssets.DarkerSprite.LoadAsset(),
                };
            }
        }
    }

    public override void OnVotingComplete()
    {
        RoleStubs.RoleBehaviourOnVotingComplete(this);

        if (Player.AmOwner)
        {
            meetingMenu.HideButtons();
        }
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);

        if (Shielded != null)
        {
            stringB.Append(CultureInfo.InvariantCulture, $"\n<b>被上盾: </b>{Color.white.ToTextColor()}{Shielded.Data.PlayerName}</color>");
        }

        return stringB;
    }

    public void Clear()
    {
        SetShieldedPlayer(null);
    }

    public override void OnDeath(DeathReason reason)
    {
        RoleStubs.RoleBehaviourOnDeath(this, reason);

        Clear();
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        Clear();

        if (Player.AmOwner)
        {
            meetingMenu?.Dispose();
            meetingMenu = null!;
        }
    }

    public void FixedUpdate()
    {
        if (Player == null || Player.Data.Role is not MedicRole) return;
        if (Shielded != null && Shielded.HasDied())
            Clear();
    }

    public void SetShieldedPlayer(PlayerControl? player)
    {
        Shielded?.RemoveModifier<MedicShieldModifier>();

        Shielded = player;

        Shielded?.AddModifier<MedicShieldModifier>(Player);
    }

    public void Report(byte deadPlayerId)
    {
        var areReportsEnabled = OptionGroupSingleton<MedicOptions>.Instance.ShowReports;

        if (!areReportsEnabled) return;

        var matches = GameHistory.KilledPlayers.Where(x => x.VictimId == deadPlayerId).ToArray();

        DeadPlayer? killer = null;

        if (matches.Length > 0)
            killer = matches[0];

        if (killer == null)
            return;

        // Logger<TownOfUsPlugin>.Message($"CmdReportDeadBody");
        var br = new BodyReport
        {
            Killer = MiscUtils.PlayerById(killer.KillerId),
            Reporter = Player,
            Body = MiscUtils.PlayerById(killer.VictimId),
            KillAge = (float)(DateTime.UtcNow - killer.KillTime).TotalMilliseconds,
        };

        var reportMsg = BodyReport.ParseMedicReport(br);

        if (string.IsNullOrWhiteSpace(reportMsg))
            return;

        var title = $"<color=#{TownOfUsColors.Medic.ToHtmlStringRGBA()}>法医报告</color>";
        var reported = Player;
        if (br.Body != null) reported = br.Body;
        MiscUtils.AddFakeChat(reported.Data, title, reportMsg, false, true);
    }

    public static string GetColorTypeForPlayer(PlayerControl player)
    {
        var colors = new Dictionary<int, string>
        {
            { 0, "深色" }, // 红
            { 1, "深色" }, // 蓝
            { 2, "深色" }, // 绿
            { 3, "浅色" }, // 粉
            { 4, "浅色" }, // 橙
            { 5, "浅色" }, // 黄
            { 6, "深色" }, // 黑
            { 7, "浅色" }, // 白
            { 8, "深色" }, // 紫
            { 9, "深色" }, // 棕
            { 10, "浅色" }, // 青
            { 11, "浅色" }, // 黄绿
            { 12, "深色" }, // 栗
            { 13, "浅色" }, // 玫红
            { 14, "浅色" }, // 香蕉
            { 15, "深色" }, // 灰
            { 16, "深色" }, // 棕褐
            { 17, "浅色" }, // 珊瑚
            { 18, "深色" }, // 西瓜
            { 19, "深色" }, // 巧克力
            { 20, "浅色" }, // 天蓝
            { 21, "浅色" }, // 米色
            { 22, "深色" }, // 洋红
            { 23, "浅色" }, // 绿松石
            { 24, "浅色" }, // 淡紫
            { 25, "深色" }, // 橄榄
            { 26, "浅色" }, // 蔚蓝
            { 27, "深色" }, // 李子
            { 28, "深色" }, // 丛林
            { 29, "浅色" }, // 薄荷
            { 30, "浅色" }, // 黄绿
            { 31, "深色" }, // 澳门
            { 32, "浅色" }, // 金色
            { 33, "深色" }, // 黄褐
            { 34, "浅色" }, // 彩虹
        };

        var typeOfColor = colors[player.Data.DefaultOutfit.ColorId];

        return typeOfColor;
    }

    public static void DangerAnim()
    {
        Coroutines.Start(MiscUtils.CoFlash(new Color(0f, 0.5f, 0f, 1f)));
    }
    public static void OnRoundStart()
    {
        CustomButtonSingleton<MedicShieldButton>.Instance.CanChangeTarget = OptionGroupSingleton<MedicOptions>.Instance.ChangeTarget;
    }

    [MethodRpc((uint)TownOfUsRpc.MedicShield, SendImmediately = true)]
    public static void RpcMedicShield(PlayerControl medic, PlayerControl target)
    {
        if (medic.Data.Role is not MedicRole)
        {
            Logger<TownOfUsPlugin>.Error("RpcMedicShield - Invalid medic");
            return;
        }

        var role = medic.GetRole<MedicRole>();

        role?.SetShieldedPlayer(target);
    }

    [MethodRpc((uint)TownOfUsRpc.ClearMedicShield, SendImmediately = true)]
    public static void RpcClearMedicShield(PlayerControl medic)
    {
        ClearMedicShield(medic);
    }
    public static void ClearMedicShield(PlayerControl medic)
    {
        if (medic.Data.Role is not MedicRole)
        {
            Logger<TownOfUsPlugin>.Error("ClearMedicShield - Invalid medic");
            return;
        }

        var role = medic.GetRole<MedicRole>();

        role?.SetShieldedPlayer(null);
    }

    [MethodRpc((uint)TownOfUsRpc.MedicShieldAttacked, SendImmediately = true)]
    public static void RpcMedicShieldAttacked(PlayerControl medic, PlayerControl source, PlayerControl shielded)
    {
        if (medic.Data.Role is not MedicRole)
        {
            Logger<TownOfUsPlugin>.Error("RpcMedicShieldAttacked - Invalid medic");
            return;
        }

        if (PlayerControl.LocalPlayer.PlayerId == source.PlayerId)
            Coroutines.Start(MiscUtils.CoFlash(new Color(0f, 0.5f, 0f, 1f)));

        var shieldNotify = OptionGroupSingleton<MedicOptions>.Instance.WhoGetsNotification;

        if (shielded.AmOwner && shieldNotify == MedicOption.Shielded)
        {
            DangerAnim();
        }

        if (medic.AmOwner && shieldNotify == MedicOption.Medic)
        {
            DangerAnim();
        }

        if (source.AmOwner)
        {
            DangerAnim();
        }

        if (shieldNotify == MedicOption.Everyone && !source.AmOwner)
        {
            DangerAnim();
        }

        var shieldBreaks = OptionGroupSingleton<MedicOptions>.Instance.ShieldBreaks;

        if (shieldBreaks)
        {
            var role = medic.GetRole<MedicRole>();
            role?.SetShieldedPlayer(null);
        }
    }

    public string GetAdvancedDescription()
    {
        return "法医是一名船员保护型角色，可以为一名玩家加护盾，保护其免受他人击杀。"
            + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new ("护盾",
            "为一名玩家加护盾，保护其免受他人击杀",
            TouCrewAssets.MedicSprite)    
    ];
}
