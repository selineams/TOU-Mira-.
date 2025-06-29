using System.Globalization;
using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class HunterRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITouCrewRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "巡猎者";
    public string RoleDescription => "盯梢<color=#FF0000FF>伪装者</color>";
    public string RoleLongDescription => "盯梢玩家异常的行为并击杀坏人，但不要错杀船员";
    public Color RoleColor => TownOfUsColors.Hunter;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateKilling;
    public DoomableType DoomHintType => DoomableType.Hunter;
    public bool IsPowerCrew => CaughtPlayers.Any(x => !x.HasDied()); // Disable end game checks if a Hunter has alive targets
    public override bool IsAffectedByComms => false;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Hunter,
        IntroSound = TouAudio.OtherIntroSound,
    };

    public PlayerControl? LastVoted { get; set; }

    [HideFromIl2Cpp]
    public List<PlayerControl> CaughtPlayers { get; } = [];

    [MethodRpc((uint)TownOfUsRpc.CatchPlayer, SendImmediately = true)]
    public static void RpcCatchPlayer(PlayerControl hunter, PlayerControl source)
    {
        if (hunter.Data.Role is not HunterRole role)
        {
            Logger<TownOfUsPlugin>.Error("RpcCatchPlayer - Invalid hunter");
            return;
        }

        if (!role.CaughtPlayers.Contains(source))
        {
            role.CaughtPlayers.Add(source);

            if (hunter.AmOwner)
            {
                Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Hunter));

                CustomButtonSingleton<HunterStalkButton>.Instance.ResetCooldownAndOrEffect();
            }
        }
    }

    public static void Retribution(PlayerControl hunter, PlayerControl target)
    {
        if (hunter.Data.Role is not HunterRole)
        {
            Logger<TownOfUsPlugin>.Error("RpcCatchPlayer - Invalid hunter");
            return;
        }

        if (hunter.AmOwner) hunter.RpcCustomMurder(target, resetKillTimer: false, createDeadBody: false, teleportMurderer: false, showKillAnim: false, playKillSound: false);
        // this sound normally plays on the source only
        if (!hunter.AmOwner)
        {
            SoundManager.Instance.PlaySound(hunter.KillSfx, false, 0.8f);
        }
        // this kill animations normally plays on the target only
        if (!target.AmOwner)
        {
            HudManager.Instance.KillOverlay.ShowKillAnimation(hunter.Data, target.Data);
        }
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);
        var stalkedPlayer = ModifierUtils.GetPlayersWithModifier<HunterStalkedModifier>(x => x.Hunter == PlayerControl.LocalPlayer).FirstOrDefault();
        var stalked = (stalkedPlayer != null && !stalkedPlayer.HasDied()) ? stalkedPlayer.Data.PlayerName : "无人";
        stringB.AppendLine(CultureInfo.InvariantCulture, $"盯梢: <b>{stalked}</b>");
        if (CaughtPlayers.Count != 0) stringB.AppendLine(CultureInfo.InvariantCulture, $"<b>被抓包的玩家:</b>");
        foreach (var player in CaughtPlayers)
        {
            var newText = $"<b><size=80%>{player.Data.PlayerName}</size></b>";
            stringB.AppendLine(CultureInfo.InvariantCulture, $"{newText}");
        }

        return stringB;
    }
    
    public string GetAdvancedDescription()
    {
        return
            "巡猎者是一名船员击杀型角色，可以在回合中盯梢玩家。"
                + "如果被盯梢的玩家使用技能，巡猎者可以在游戏中随时击杀他们（包括船员）。 "
               + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("盯梢",
            $"选择一个目标进行盯梢。你可以盯梢{OptionGroupSingleton<HunterOptions>.Instance.StalkUses} 名玩家。 " +
            $"如果他们在被盯梢期间使用技能，将被加入你的击杀名单并可以被击杀。",
            TouCrewAssets.StalkButtonSprite),
    ];

}
