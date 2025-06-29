using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Modules.Wiki;
using TownOfUs.Utilities;
using UnityEngine;


namespace TownOfUs.Roles.Crewmate;

public sealed class MysticRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "灵媒";
    public string RoleDescription => "知晓击杀发生的时间与地点";
    public string RoleLongDescription => "感知击杀发生的时间与地点";
    public Color RoleColor => TownOfUsColors.Mystic;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;
    public DoomableType DoomHintType => DoomableType.Perception;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Mystic,
        IntroSound = TouAudio.MediumIntroSound,
    };

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return "灵媒是一名船员调查型角色，在有人死亡时会收到提示。"
            + MiscUtils.AppendOptionsText(GetType());
    }
}
