using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Hud;
using MiraAPI.Roles;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Modules.Wiki;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Impostor;

public sealed class VenererRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "狩猎者";
    public string RoleDescription => "每次击杀能力更强";
    public string RoleLongDescription => "击杀玩家以解锁能力加成";
    public Color RoleColor => TownOfUsColors.Impostor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public DoomableType DoomHintType => DoomableType.Trickster;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Venerer,
    };

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);

        CustomButtonSingleton<VenererAbilityButton>.Instance.UpdateAbility(VenererAbility.None);
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    public string GetAdvancedDescription()
    {
        return "狩猎者是一名伪装者隐蔽型角色，可以通过击杀玩家获得新能力，防止被抓住！但每次获得新能力时会立即使用，并且能力会叠加。"
               + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("伪装",
            $"能力第一阶段。\n你在所有玩家眼中会变成灰豆，可以悄悄离开击杀现场。",
            TouImpAssets.CamouflageSprite),
        new("疾跑",
            $"能力第二阶段。\n你在伪装状态下会获得闪电侠般的速度。",
            TouImpAssets.SprintSprite),
        new("冰封",
            $"能力最终阶段。\n你会减缓周围玩家的速度，同时自己依然快速且处于伪装状态。",
            TouImpAssets.FreezeSprite),
    ];
}

public enum VenererAbility
{
    None,
    Camouflage,
    Sprint,
    Freeze,
}
