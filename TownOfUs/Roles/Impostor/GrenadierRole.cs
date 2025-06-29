using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Impostor;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Impostor;

public sealed class GrenadierRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "掷弹兵";
    public string RoleDescription => "干扰船员视野";
    public string RoleLongDescription => "致盲船员，便于隐蔽击杀";
    public Color RoleColor => TownOfUsColors.Impostor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public DoomableType DoomHintType => DoomableType.Protective;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Grenadier,
        CanUseVent = OptionGroupSingleton<GrenadierOptions>.Instance.CanVent,
    };

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return "掷弹兵是一名伪装者隐蔽型角色，可以投掷手雷致盲所有其他玩家。"
            + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("闪光",
            "投掷手雷，致盲范围内所有玩家。",
            TouImpAssets.FlashSprite)    
    ];
}
