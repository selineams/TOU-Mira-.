using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class VenererOptions : AbstractOptionGroup<VenererRole>
{
    public override string GroupName => "狩猎者";

    [ModdedNumberOption("技能冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float AbilityCooldown { get; set; } = 10f;

    [ModdedNumberOption("技能持续时间", 5f, 15f, suffixType: MiraNumberSuffixes.Seconds)]
    public float AbilityDuration { get; set; } = 15f;

    [ModdedNumberOption("冲刺速度", 1.05f, 2.5f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float NumSprintSpeed { get; set; } = 2f;

    [ModdedNumberOption("最低冻结速度", 0.05f, 0.75f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float MinFreezeSpeed { get; set; } = 0.25f;

    [ModdedNumberOption("冻结半径", 0.25f, 5f, 0.25f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float FreezeRadius { get; set; } = 5f;
}
