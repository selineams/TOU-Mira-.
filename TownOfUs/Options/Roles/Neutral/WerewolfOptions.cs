using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class WerewolfOptions : AbstractOptionGroup<WerewolfRole>
{
    public override string GroupName => "月下狼人";

    [ModdedNumberOption("狂暴冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float RampageCooldown { get; set; } = 20f;

    [ModdedNumberOption("狂暴持续时间", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float RampageDuration { get; set; } = 15f;

    [ModdedNumberOption("狂暴击杀冷却", 0.5f, 15f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float RampageKillCooldown { get; set; } = 2f;

    [ModdedToggleOption("月下狼人狂暴时可进通风口")]
    public bool CanVent { get; set; } = true;
}
