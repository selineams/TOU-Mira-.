using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class BomberOptions : AbstractOptionGroup<BomberRole>
{
    public override string GroupName => "爆破手";

    [ModdedNumberOption("每局可用炸弹次数", 0f, 15f, 1f, MiraNumberSuffixes.None, "0", zeroInfinity: true)]
    public float MaxBombs { get; set; } = 15f;

    [ModdedNumberOption("引爆延迟", 1f, 15f, 1f, MiraNumberSuffixes.Seconds)]
    public float DetonateDelay { get; set; } = 5f;

    [ModdedNumberOption("引爆半径", 0.05f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float DetonateRadius { get; set; } = 0.75f;

    [ModdedNumberOption("单次引爆最大击杀数", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxKillsInDetonation { get; set; } = 15f;

    [ModdedToggleOption("所有伪装者可见炸弹")]
    public bool AllImpsSeeBomb { get; set; } = true;

    [ModdedToggleOption("爆破手可进通风口")]
    public bool BomberVent { get; set; } = true;
}
