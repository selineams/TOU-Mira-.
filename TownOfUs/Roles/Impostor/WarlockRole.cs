using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Hud;
using MiraAPI.Roles;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Modules.Wiki;
using TownOfUs.Patches.Stubs;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Impostor;

public sealed class WarlockRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    public string RoleName => "巫术师";
    public string RoleDescription => "蓄力击杀，连环收割";
    public string RoleLongDescription => "蓄力后可短时间内连续击杀多名玩家";
    public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<VeteranRole>());
    public Color RoleColor => TownOfUsColors.Impostor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorKilling;
    public DoomableType DoomHintType => DoomableType.Relentless;
    public CustomRoleConfiguration Configuration => new(this)
    {
        UseVanillaKillButton = false,
        IntroSound = TouAudio.WarlockIntroSound,
        Icon = TouRoleIcons.Warlock,
    };

    public override void OnMeetingStart()
    {
        RoleStubs.RoleBehaviourOnMeetingStart(this);
        
        CustomButtonSingleton<WarlockKillButton>.Instance.Charge = 0f;
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    public string GetAdvancedDescription()
    {
        return "巫术师是一名伪装者击杀型角色，可以蓄力攻击，快速收割船员。"
               + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("击杀",
            $"你的普通击杀键被三阶段替代：冷却中、蓄力中、已蓄力。 " +
            "冷却时无法击杀，蓄力中时可击杀但会重置蓄力， " +
            "蓄力完成后可短时间内连续击杀多名玩家。",
            TouAssets.KillSprite),
    ];
}
