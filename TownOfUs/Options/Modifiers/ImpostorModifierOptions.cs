using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using UnityEngine;

namespace TownOfUs.Options.Modifiers;

public sealed class ImpostorModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "伪装者附加特性";
    public override Color GroupColor => Palette.ImpostorRoleHeaderRed;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 3;

    [ModdedNumberOption("分散者数量", 0, 5, 1)]
    public float DisperserAmount { get; set; } = 1;
    public ModdedNumberOption DisperserChance { get; } = new ModdedNumberOption("分散者生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.DisperserAmount > 0,
    };

    [ModdedNumberOption("专业刺客数量", 0, 5, 1)]
    public float DoubleShotAmount { get; set; } = 1;
    public ModdedNumberOption DoubleShotChance { get; } = new ModdedNumberOption("专业刺客生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.DoubleShotAmount > 0,
    };

    [ModdedNumberOption("破坏者数量", 0, 5, 1)]
    public float SaboteurAmount { get; set; } = 1;
    public ModdedNumberOption SaboteurChance { get; } = new ModdedNumberOption("破坏者生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.SaboteurAmount > 0,
    };

    [ModdedNumberOption("通感者数量", 0, 5, 1)]
    public float TelepathAmount { get; set; } = 1;
    public ModdedNumberOption TelepathChance { get; } = new ModdedNumberOption("通感者生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.TelepathAmount > 0,
    };

    [ModdedNumberOption("潜伏者数量", 0, 5, 1)]
    public float UnderdogAmount { get; set; } = 1;
    public ModdedNumberOption UnderdogChance { get; } = new ModdedNumberOption("潜伏者生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.UnderdogAmount > 0,
    };
}
