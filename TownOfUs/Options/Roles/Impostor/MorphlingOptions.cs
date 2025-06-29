using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class MorphlingOptions : AbstractOptionGroup<MorphlingRole>
{
    public override string GroupName => "化形者";

    [ModdedNumberOption("每局可采样次数", 0f, 15f, 5f, MiraNumberSuffixes.None, "0", zeroInfinity: true)]
    public float MaxSamples { get; set; } = 15f;
    [ModdedNumberOption("每回合可变身次数", 0f, 10f, 1f, MiraNumberSuffixes.None, "0", zeroInfinity: true)]
    public float MaxMorphs { get; set; } = 10f;

    [ModdedNumberOption("化形者冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float MorphlingCooldown { get; set; } = 15f;

    [ModdedNumberOption("化形持续时间", 5f, 15f, 1f, MiraNumberSuffixes.Seconds)]
    public float MorphlingDuration { get; set; } = 15f;

    [ModdedToggleOption("化形者可进通风口")]
    public bool MorphlingVent { get; set; } = true;
}
