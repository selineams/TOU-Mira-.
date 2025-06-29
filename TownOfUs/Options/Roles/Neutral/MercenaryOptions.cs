using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class MercenaryOptions : AbstractOptionGroup<MercenaryRole>
{
    public override string GroupName => "雇佣兵";

    [ModdedNumberOption("雇佣兵守卫冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float GuardCooldown { get; set; } = 10f;

    [ModdedNumberOption("雇佣兵最大守卫次数", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxUses { get; set; } = 15f;

    [ModdedNumberOption("贿赂花费", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float BribeCost { get; set; } = 2f;
}
