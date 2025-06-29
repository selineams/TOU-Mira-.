using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Roles;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class SpyRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "特工";
    public string RoleDescription => "四处侦查，发现情报";
    public string RoleLongDescription => "在管理台获得额外信息";
    public Color RoleColor => TownOfUsColors.Spy;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;
    public DoomableType DoomHintType => DoomableType.Perception;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Spy,
        IntroSound = TouAudio.SpyIntroSound,
    };
    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);
        if (Player.AmOwner)
        {
            CustomButtonSingleton<SpyAdminTableRoleButton>.Instance.AvailableCharge = OptionGroupSingleton<SpyOptions>.Instance.StartingCharge.Value;
        }
    }
    public static void OnRoundStart()
    {
        CustomButtonSingleton<SpyAdminTableRoleButton>.Instance.AvailableCharge += OptionGroupSingleton<SpyOptions>.Instance.RoundCharge.Value;
    }
    public static void OnTaskComplete()
    {
        CustomButtonSingleton<SpyAdminTableRoleButton>.Instance.AvailableCharge += OptionGroupSingleton<SpyOptions>.Instance.TaskCharge.Value;
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    
    public string GetAdvancedDescription()
    {
        return "特工是一名船员调查型角色，可以在管理台获得额外信息。不仅能看到每个房间的人数，还能看到每个房间里都有谁。"
            + MiscUtils.AppendOptionsText(GetType());
    }
}
