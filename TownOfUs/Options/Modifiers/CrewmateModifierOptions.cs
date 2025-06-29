using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using UnityEngine;

namespace TownOfUs.Options.Modifiers;

public sealed class CrewmateModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "船员附加特性";
    public override Color GroupColor => Palette.CrewmateRoleHeaderBlue;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 2;

    [ModdedNumberOption("余波数量", 0, 5, 1)]
    public float AftermathAmount { get; set; } = 1;
    public ModdedNumberOption AftermathChance { get; } = new ModdedNumberOption("余波生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.AftermathAmount > 0,
    };
    
    [ModdedNumberOption("诱饵数量", 0, 5, 1)]
    public float BaitAmount { get; set; } = 1;
    public ModdedNumberOption BaitChance { get; } = new ModdedNumberOption("诱饵生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.BaitAmount > 0,
    };
    [ModdedNumberOption("名人数量", 0, 1, 1)]
    public float CelebrityAmount { get; set; } = 1;
    public ModdedNumberOption CelebrityChance { get; } = new ModdedNumberOption("名人生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.CelebrityAmount > 0,
    };

    [ModdedNumberOption("病人数量", 0, 5, 1)]
    public float DiseasedAmount { get; set; } = 1;
    public ModdedNumberOption DiseasedChance { get; } = new ModdedNumberOption("病人生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.DiseasedAmount > 0,
    };

    [ModdedNumberOption("雪人数量", 0, 5, 1)]
    public float FrostyAmount { get; set; } = 1;
    public ModdedNumberOption FrostyChance { get; } = new ModdedNumberOption("雪人生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.FrostyAmount > 0,
    };

    [ModdedNumberOption("调查员数量", 0, 5, 1)]
    public float InvestigatorAmount { get; set; } = 1;
    public ModdedNumberOption InvestigatorChance { get; } = new ModdedNumberOption("调查员生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.InvestigatorAmount > 0,
    };

    [ModdedNumberOption("多线程数量", 0, 5, 1)]
    public float MultitaskerAmount { get; set; } = 1;
    public ModdedNumberOption MultitaskerChance { get; } = new ModdedNumberOption("多线程生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.MultitaskerAmount > 0,
    };
    [ModdedNumberOption("大嗓门数量", 0, 5, 1)]
    public float NoisemakerAmount { get; set; } = 1;
    public ModdedNumberOption NoisemakerChance { get; } = new ModdedNumberOption("大嗓门生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.NoisemakerAmount > 0,
    };
    [ModdedNumberOption("监控员数量", 0, 5, 1)]
    public float OperativeAmount { get; set; } = 1;
    public ModdedNumberOption OperativeChance { get; } = new ModdedNumberOption("监控员生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.OperativeAmount > 0,
    };

    [ModdedNumberOption("腐烂数量", 0, 5, 1)]
    public float RottingAmount { get; set; } = 1;
    public ModdedNumberOption RottingChance { get; } = new ModdedNumberOption("腐烂生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.RottingAmount > 0,
    };
    [ModdedNumberOption("科学家数量", 0, 5, 1)]
    public float ScientistAmount { get; set; } = 1;
    public ModdedNumberOption ScientistChance { get; } = new ModdedNumberOption("科学家生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.ScientistAmount > 0,
    };

    [ModdedNumberOption("侦察兵数量", 0, 5, 1)]
    public float ScoutAmount { get; set; } = 1;
    public ModdedNumberOption ScoutChance { get; } = new ModdedNumberOption("侦察兵生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.ScoutAmount > 0,
    };

    [ModdedNumberOption("特工数量", 0, 5, 1)]
    public float SpyAmount { get; set; } = 1;
    public ModdedNumberOption SpyChance { get; } = new ModdedNumberOption("特工生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.SpyAmount > 0,
    };

    [ModdedNumberOption("任务大师数量", 0, 5, 1)]
    public float TaskmasterAmount { get; set; } = 1;
    public ModdedNumberOption TaskmasterChance { get; } = new ModdedNumberOption("任务大师生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.TaskmasterAmount > 0,
    };

    [ModdedNumberOption("火炬数量", 0, 5, 1)]
    public float TorchAmount { get; set; } = 0;
    public ModdedNumberOption TorchChance { get; } = new ModdedNumberOption("火炬生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.TorchAmount > 0,
    };
}
