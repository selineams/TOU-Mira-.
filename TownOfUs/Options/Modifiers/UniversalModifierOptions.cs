using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace TownOfUs.Options.Modifiers;

public sealed class UniversalModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "通用附加特性";
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 1;

    [ModdedNumberOption("执钮人数量", 0, 1, 1)]
    public float ButtonBarryAmount { get; set; } = 1;
    public ModdedNumberOption ButtonBarryChance { get; } = new ModdedNumberOption("执钮人生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.ButtonBarryAmount > 0,
    };
    
    [ModdedNumberOption("闪电侠数量", 0, 5, 1)]
    public float FlashAmount { get; set; } = 1;
    public ModdedNumberOption FlashChance { get; } = new ModdedNumberOption("闪电侠生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.FlashAmount > 0,
    };

    [ModdedNumberOption("巨人数量", 0, 5, 1)]
    public float GiantAmount { get; set; } = 1;
    public ModdedNumberOption GiantChance { get; } = new ModdedNumberOption("巨人生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.GiantAmount > 0,
    };

    [ModdedNumberOption("磐石数量", 0, 5, 1)]
    public float ImmovableAmount { get; set; } = 1;
    public ModdedNumberOption ImmovableChance { get; } = new ModdedNumberOption("磐石生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.ImmovableAmount > 0,
    };

    [ModdedNumberOption("迷你数量", 0, 5, 1)]
    public float MiniAmount { get; set; } = 1;
    public ModdedNumberOption MiniChance { get; } = new ModdedNumberOption("迷你生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.MiniAmount > 0,
    };

    [ModdedNumberOption("雷达数量", 0, 5, 1)]
    public float RadarAmount { get; set; } = 1;
    public ModdedNumberOption RadarChance { get; } = new ModdedNumberOption("雷达生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.RadarAmount > 0,
    };

    [ModdedNumberOption("卫星数量", 0, 5, 1)]
    public float SatelliteAmount { get; set; } = 1;
    public ModdedNumberOption SatelliteChance { get; } = new ModdedNumberOption("卫星生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.SatelliteAmount > 0,
    };

    [ModdedNumberOption("变色龙数量", 0, 5, 1)]
    public float ShyAmount { get; set; } = 1;
    public ModdedNumberOption ShyChance { get; } = new ModdedNumberOption("变色龙生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.ShyAmount > 0,
    };

    [ModdedNumberOption("第六感数量", 0, 5, 1)]
    public float SixthSenseAmount { get; set; } = 1;
    public ModdedNumberOption SixthSenseChance { get; } = new ModdedNumberOption("第六感生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.SixthSenseAmount > 0,
    };

    [ModdedNumberOption("掘墓者数量", 0, 5, 1)]
    public float SleuthAmount { get; set; } = 1;
    public ModdedNumberOption SleuthChance { get; } = new ModdedNumberOption("掘墓者生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.SleuthAmount > 0,
    };

    [ModdedNumberOption("破平者数量", 0, 1, 1)]
    public float TiebreakerAmount { get; set; } = 1;
    public ModdedNumberOption TiebreakerChance { get; } = new ModdedNumberOption("破平者生成概率", 100f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.TiebreakerAmount > 0,
    };
}
