using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class PlaguebearerOptions : AbstractOptionGroup<PlaguebearerRole>
{
    public override string GroupName => "瘟疫之源";

    [ModdedNumberOption("一开始生成万疫之神概率", 0, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float PestChance { get; set; } = 70f;

    [ModdedNumberOption("感染冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float InfectCooldown { get; set; } = 10f;

    [ModdedNumberOption("万疫之神击杀冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float PestKillCooldown { get; set; } = 25f;

    [ModdedToggleOption("万疫之神可进通风口")]
    public bool CanVent { get; set; } = true;
}
