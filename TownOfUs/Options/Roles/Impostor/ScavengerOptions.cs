using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class ScavengerOptions : AbstractOptionGroup<ScavengerRole>
{
    public override string GroupName => "赏金猎人";

    [ModdedNumberOption("赏金持续时间", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ScavengeDuration { get; set; } = 30f;

    [ModdedNumberOption("每次击杀增加赏金持续时间", 5f, 15f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float ScavengeIncreaseDuration { get; set; } = 15f;

    [ModdedNumberOption("正确击杀后的赏金冷却", 5f, 15f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float ScavengeCorrectKillCooldown { get; set; } = 15f;

    [ModdedNumberOption("错误击杀后的冷却倍率", 1.25f, 5f, 0.25f, MiraNumberSuffixes.Multiplier)]
    public float ScavengeIncorrectKillCooldown { get; set; } = 1.25f;
}
