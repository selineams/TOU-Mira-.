using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class HypnotistOptions : AbstractOptionGroup<HypnotistRole>
{
    public override string GroupName => "催眠师";

    [ModdedNumberOption("催眠冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float HypnotiseCooldown { get; set; } = 10f;
    [ModdedToggleOption("催眠师可与队友一起击杀")]
    public bool HypnoKill { get; set; } = true;
}
