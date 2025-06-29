using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class VeteranOptions : AbstractOptionGroup<VeteranRole>
{
    public override string GroupName => "老兵";

    [ModdedNumberOption("警戒冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float AlertCooldown { get; set; } = 25f;

    [ModdedNumberOption("警戒持续时间", 5f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float AlertDuration { get; set; } = 10f;

    [ModdedNumberOption("最大警戒次数", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxNumAlerts { get; set; } = 5f;

    [ModdedToggleOption("警戒时可被击杀")]
    public bool KilledOnAlert { get; set; } = false;

    [ModdedToggleOption("完成任务可获得更多警戒次数")]
    public bool TaskUses { get; set; } = true;
}
