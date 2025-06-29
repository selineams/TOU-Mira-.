using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules.Wiki;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class InvestigatorRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "调查员";
    public string RoleDescription => "通过查看足迹找出所有伪装者。";
    public string RoleLongDescription => "你可以看到所有人的足迹。";
    public Color RoleColor => TownOfUsColors.Investigator;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;
    public DoomableType DoomHintType => DoomableType.Hunter;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Investigator,
        IntroSound = TouAudio.SpyIntroSound,
    };

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);

        if (!player.AmOwner)
        {
            return;
        }

        Helpers.GetAlivePlayers().Where(plr => !plr.HasModifier<FootstepsModifier>())
            .ToList().ForEach(plr => plr.GetModifierComponent().AddModifier<FootstepsModifier>());
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        if (!targetPlayer.AmOwner)
        {
            return;
        }

        PlayerControl.AllPlayerControls.ToArray().Where(plr => plr.HasModifier<FootstepsModifier>())
            .ToList().ForEach(plr => plr.GetModifierComponent().RemoveModifier<FootstepsModifier>());
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    
    public string GetAdvancedDescription()
    {
        return
            "调查员是一名船员调查型角色，可以在游戏中看到所有玩家的足迹。隐身状态下的玩家足迹对调查员不可见。"
            + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
