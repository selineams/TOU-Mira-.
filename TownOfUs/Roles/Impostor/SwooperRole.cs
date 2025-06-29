using AmongUs.GameOptions;
using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using TownOfUs.Options.Roles.Impostor;
using UnityEngine;
using TownOfUs.Modules.Wiki;
using TownOfUs.Utilities;

namespace TownOfUs.Roles.Impostor;

public sealed class SwooperRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "隐身人";
    public string RoleDescription => "短暂隐身";
    public string RoleLongDescription => "隐身并悄悄击杀";
    public Color RoleColor => TownOfUsColors.Impostor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public DoomableType DoomHintType => DoomableType.Hunter;
    public CustomRoleConfiguration Configuration => new(this)
    {
        CanUseVent = OptionGroupSingleton<SwooperOptions>.Instance.CanVent,
        Icon = TouRoleIcons.Swooper,
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Phantom),
    };

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return "隐身人是一名伪装者隐蔽型角色，可以短暂隐身。"
            + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("隐身",
            "对除伪装者外的所有玩家隐身。",
            TouImpAssets.SwoopSprite),
        new("解除隐身",
            "提前解除隐身，或等待隐身结束后重新现身。",
            TouImpAssets.UnswoopSprite)
    ];
}
