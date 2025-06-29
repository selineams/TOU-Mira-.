using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace TownOfUs.Options;

public sealed class AssassinOptions : AbstractOptionGroup
{
    public override string GroupName => "刺客选项";
    public override uint GroupPriority => 7;

    [ModdedNumberOption("内鬼刺客数量", 0, 4, 1, MiraNumberSuffixes.None, "0")]
    public float NumberOfImpostorAssassins { get; set; } = 1;
    public ModdedNumberOption ImpAssassinChance { get; } = new ModdedNumberOption("内鬼刺客概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<AssassinOptions>.Instance.NumberOfImpostorAssassins > 0,
    };

    [ModdedNumberOption("中立刺客数量", 0, 5, 1, MiraNumberSuffixes.None, "0")]
    public float NumberOfNeutralAssassins { get; set; } = 1;
    public ModdedNumberOption NeutAssassinChance { get; } = new ModdedNumberOption("中立刺客概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<AssassinOptions>.Instance.NumberOfNeutralAssassins > 0,
    };

    [ModdedToggleOption("失忆者变为内鬼获得刺客能力")]
    public bool AmneTurnImpAssassin { get; set; } = true;

    [ModdedToggleOption("失忆者变为中立杀手获得刺客能力")]
    public bool AmneTurnNeutAssassin { get; set; } = true;

    [ModdedToggleOption("背叛者获得刺客能力")]
    public bool TraitorCanAssassin { get; set; } = true;

    [ModdedNumberOption("刺客可用击杀次数", 1, 15, 1, MiraNumberSuffixes.None, "0")]
    public float AssassinKills { get; set; } = 5;

    [ModdedToggleOption("刺客每次会议可多次击杀")]
    public bool AssassinMultiKill { get; set; } = true;

    [ModdedToggleOption("刺客可猜“船员”身份")]
    public bool AssassinCrewmateGuess { get; set; } = false;

    [ModdedToggleOption("刺客可猜船员侦查职业")]
    public bool AssassinGuessInvest { get; set; } = true;

    [ModdedToggleOption("刺客可猜中立善良职业")]
    public bool AssassinGuessNeutralBenign { get; set; } = true;

    [ModdedToggleOption("刺客可猜中立邪恶职业")]
    public bool AssassinGuessNeutralEvil { get; set; } = true;

    [ModdedToggleOption("刺客可猜中立杀手职业")]
    public bool AssassinGuessNeutralKilling { get; set; } = true;

    [ModdedToggleOption("刺客可猜内鬼职业")]
    public bool AssassinGuessImpostors { get; set; } = true;

    [ModdedToggleOption("刺客可猜船员附加特性")]
    public bool AssassinGuessCrewModifiers { get; set; } = true;

    public ModdedToggleOption AssassinGuessInvModifier { get; } = new ModdedToggleOption("刺客可猜调查员附加特性", true)
    {
        Visible = () => OptionGroupSingleton<AssassinOptions>.Instance.AssassinGuessCrewModifiers,
    };

    public ModdedToggleOption AssassinGuessSpyModifier { get; } = new ModdedToggleOption("刺客可猜特工附加特性", true)
    {
        Visible = () => OptionGroupSingleton<AssassinOptions>.Instance.AssassinGuessCrewModifiers,
    };

    [ModdedToggleOption("刺客可猜联盟关系")]
    public bool AssassinGuessAlliances { get; set; } = false;

}
