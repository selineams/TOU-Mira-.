using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class InquisitorOptions : AbstractOptionGroup<InquisitorRole>
{
    public override string GroupName => "审判官";

    [ModdedNumberOption("裁决冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float VanquishCooldown { get; set; } = 20f;

    [ModdedNumberOption("审问冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float InquireCooldown { get; set; } = 20f;

    [ModdedToggleOption("审判官胜利继续游戏")]
    public bool StallGame { get; set; } = false;

    [ModdedToggleOption("审判官无法审问")]
    public bool CantInquire { get; set; } = false;

    [ModdedNumberOption("最大审问次数", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxUses { get; set; } = 15f;

    [ModdedNumberOption("所需消灭异端数量", 3f, 5f, 1f, MiraNumberSuffixes.None, "0")]
    public float AmountOfHeretics { get; set; } = 3f;
}
