using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class GuardianAngelOptions : AbstractOptionGroup<GuardianAngelTouRole>
{
    public override string GroupName => "守护天使";

    [ModdedNumberOption("守护冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ProtectCooldown { get; set; } = 15f;

    [ModdedNumberOption("守护持续时间", 5f, 15f, 1f, MiraNumberSuffixes.Seconds)]
    public float ProtectDuration { get; set; } = 15f;

    [ModdedNumberOption("最大守护次数", 1, 15, 1, MiraNumberSuffixes.None, "0")]
    public float MaxProtects { get; set; } = 15;

    [ModdedEnumOption("显示被守护玩家", typeof(ProtectOptions), ["守护天使", "自身+守护天使", "所有人"])]
    public ProtectOptions ShowProtect { get; set; } = ProtectOptions.SelfAndGA;

    [ModdedEnumOption("目标死亡后，守护天使变为", typeof(BecomeOptions))]
    public BecomeOptions OnTargetDeath { get; set; } = BecomeOptions.Mercenary;

    [ModdedToggleOption("守护天使知晓目标职业")]
    public bool GAKnowsTargetRole { get; set; } = true;

    [ModdedNumberOption("目标为邪恶阵营概率", 0f, 100f, 10f, MiraNumberSuffixes.Percent, "0")]
    public float EvilTargetPercent { get; set; } = 60f;
}

public enum ProtectOptions
{
    GA,
    SelfAndGA,
    Everyone,
}

public enum BecomeOptions
{
    Crew,
    Amnesiac,
    Survivor,
    Mercenary,
    Jester,
}
