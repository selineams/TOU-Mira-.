using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class TransporterOptions : AbstractOptionGroup<TransporterRole>
{
    public override string GroupName => "传送师";

    [ModdedNumberOption("传送冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float TransporterCooldown { get; set; } = 15f;

    [ModdedNumberOption("最大可用次数", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxNumTransports { get; set; } = 15f;

    /* [ModdedToggleOption("使用传送菜单时可移动")]
    public bool MoveWithMenu { get; set; } = true; */

    [ModdedToggleOption("传送师可用生命体征")]
    public bool CanUseVitals { get; set; } = true;
    [ModdedToggleOption("完成任务可获得更多可用次数")]
    public bool TaskUses { get; set; } = true;
}
