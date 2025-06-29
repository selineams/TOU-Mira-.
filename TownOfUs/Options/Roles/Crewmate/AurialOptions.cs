using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class AurialOptions : AbstractOptionGroup<AurialRole>
{
    public override string GroupName => "灵气探";

    [ModdedNumberOption("灵气内圈范围", 0f, 1f, 0.25f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float AuraInnerRadius { get; set; } = 0.5f;

    [ModdedNumberOption("灵气最大范围", 1f, 5f, 0.25f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float AuraOuterRadius { get; set; } = 2f;

    [ModdedNumberOption("感知持续时间", 1f, 15f, 1f, MiraNumberSuffixes.Seconds, "0")]
    public float SenseDuration { get; set; } = 10f;
}
