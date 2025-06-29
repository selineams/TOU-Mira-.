using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class TrapperOptions : AbstractOptionGroup<TrapperRole>
{
    public override string GroupName => "陷阱师";

    [ModdedNumberOption("陷阱冷却", 1f, 30f, 1f, MiraNumberSuffixes.Seconds)]
    public float TrapCooldown { get; set; } = 15f;

    [ModdedNumberOption("触发陷阱所需最短时间", 0f, 15f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float MinAmountOfTimeInTrap { get; set; } = 1f;
    
    [ModdedNumberOption("最大陷阱数量", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxTraps { get; set; } = 5f;

    [ModdedNumberOption("陷阱大小", 0.05f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float TrapSize { get; set; } = 0.25f;

    [ModdedToggleOption("每回合移除陷阱")]
    public bool TrapsRemoveOnNewRound { get; set; } = true;
    
    public ModdedToggleOption TaskUses { get; } = new ModdedToggleOption("完成任务可获得更多陷阱次数", true)
    {
        Visible = () => !OptionGroupSingleton<TrapperOptions>.Instance.TrapsRemoveOnNewRound,
    };

    [ModdedNumberOption("触发陷阱所需最少角色数", 1f, 15f, 1f, MiraNumberSuffixes.None)]
    public float MinAmountOfPlayersInTrap { get; set; } = 3f;
}
