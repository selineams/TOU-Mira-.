using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class BlackmailerOptions : AbstractOptionGroup<BlackmailerRole>
{
    public override string GroupName => "勒索者";

    [ModdedNumberOption("每局可用勒索次数", 0f, 15f, 5f, MiraNumberSuffixes.None, "0", zeroInfinity: true)]
    public float MaxBlackmails { get; set; } = 15f;

    [ModdedNumberOption("勒索冷却", 1f, 30f, suffixType: MiraNumberSuffixes.Seconds)]
    public float BlackmailCooldown { get; set; } = 5f;

    [ModdedNumberOption("允许被勒索人投票的最大存活人数", 1f, 15f)]
    public float MaxAliveForVoting { get; set; } = 5f;
    [ModdedToggleOption("可连续勒索同一人")]
    public bool BlackmailInARow { get; set; } = true;

    [ModdedToggleOption("仅目标可见勒索提示")]
    public bool OnlyTargetSeesBlackmail { get; set; } = false;
    [ModdedToggleOption("勒索者可与队友一起击杀")]
    public bool BlackmailerKill { get; set; } = true;
}
