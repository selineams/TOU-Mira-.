using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class SurvivorOptions : AbstractOptionGroup<SurvivorRole>
{
    public override string GroupName => "幸存者";

    [ModdedNumberOption("防弹衣冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float VestCooldown { get; set; } = 20f;

    [ModdedNumberOption("防弹衣持续时间", 5f, 15f, 1f, MiraNumberSuffixes.Seconds)]
    public float VestDuration { get; set; } = 15f;

    [ModdedNumberOption("最大防弹衣数量", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxVests { get; set; } = 15f;

    [ModdedToggleOption("幸存者长时间固定活动自杀")]
    public bool ScatterOn { get; set; } = false;

    [ModdedNumberOption("幸存者多久固定活动自杀", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float ScatterTimer { get; set; } = 55f;
}
