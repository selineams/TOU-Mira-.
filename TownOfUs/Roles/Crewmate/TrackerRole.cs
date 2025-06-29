using System.Globalization;
using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules.Wiki;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class TrackerTouRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "追踪者";
    public string RoleDescription => "追踪所有人的行动";
    public string RoleLongDescription => "追踪可疑玩家，观察他们的去向";
    public Color RoleColor => TownOfUsColors.Tracker;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;
    public DoomableType DoomHintType => DoomableType.Hunter;
    public override bool IsAffectedByComms => false;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Tracker,
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Tracker),
    };

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);

        var players = ModifierUtils.GetPlayersWithModifier<TrackerArrowTargetModifier>([HideFromIl2Cpp] (x) => x.Owner == Player);

        var playerControls = players as PlayerControl[] ?? players.ToArray();
        if (playerControls.Length == 0) return stringB;
        stringB.Append("\n<b>被追踪玩家:</b>");
        foreach (var plr in playerControls)
        {
            stringB.Append(CultureInfo.InvariantCulture, $"\n{plr.Data.PlayerName}");
        }

        return stringB;
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        Clear();
    }

    public void Clear()
    {
        var players = ModifierUtils.GetPlayersWithModifier<TrackerArrowTargetModifier>([HideFromIl2Cpp] (x) => x.Owner == Player);

        foreach (var player in players)
        {
            player.RemoveModifier<TrackerArrowTargetModifier>();
        }
    }
    public string GetAdvancedDescription()
    {
        return "追踪者是一名船员调查型角色，可以追踪其他玩家，在地图上大致了解他们的位置，并获得指向所有被追踪者的彩色箭头。"
               + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities => [
        new("追踪",
            "追踪一名玩家，观察他们的去向。" +
            "你会获得一个指向其位置的箭头，箭头会定期更新。" +
            "如果目标死亡，箭头会消失，或根据设置，会议后追踪会被重置。",
            TouCrewAssets.TrackSprite),
    ];
}
