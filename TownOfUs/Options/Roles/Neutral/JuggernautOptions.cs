using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class JuggernautOptions : AbstractOptionGroup<JuggernautRole>
{
    public override string GroupName => "天启";

    [ModdedNumberOption("初始击杀冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 25f;

    [ModdedNumberOption("击杀冷却缩减", 2.5f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldownReduction { get; set; } = 7.5f;

    [ModdedToggleOption("天启可进通风口")]
    public bool CanVent { get; set; } = true;
}
