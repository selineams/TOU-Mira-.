using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class VeteranRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITouCrewRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "老兵";
    public string RoleDescription => "警戒并反杀所有与你互动的人";
    public string RoleLongDescription => "警戒状态下，杀死所有与你互动的玩家。";
    public Color RoleColor => TownOfUsColors.Veteran;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateKilling;
    public DoomableType DoomHintType => DoomableType.Trickster;
    public override bool IsAffectedByComms => false;
    public bool IsPowerCrew => Alerts > 0; // Stop end game checks if the veteran can still alert
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Veteran,
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Impostor),
    };

    public int Alerts { get; set; }

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);
        Alerts = (int)OptionGroupSingleton<VeteranOptions>.Instance.MaxNumAlerts;
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    
    public string GetAdvancedDescription()
    {
        return "老兵是一名船员击杀型角色，可以进入警戒状态，杀死所有与其互动的玩家。"
            + MiscUtils.AppendOptionsText(GetType());
    }
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("警戒",
            $"老兵处于警戒状态时，任何与其互动的玩家都会被立即击杀，瘟疫使者和有护盾的玩家除外，他们会无视攻击。",
            TouCrewAssets.AlertSprite),
    ];
}
