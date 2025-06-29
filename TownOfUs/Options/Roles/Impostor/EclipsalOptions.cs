using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class EclipsalOptions : AbstractOptionGroup<EclipsalRole>
{
    public override string GroupName => "蚀影者";

    [ModdedNumberOption("致盲冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float BlindCooldown { get; set; } = 20f;

    [ModdedNumberOption("致盲持续时间", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float BlindDuration { get; set; } = 25f;

    [ModdedNumberOption("致盲半径", 0.25f, 5f, 0.25f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float BlindRadius { get; set; } = 3f;
}
