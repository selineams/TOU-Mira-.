using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace TownOfUs.Options;

public sealed class TownOfUsMapOptions : AbstractOptionGroup
{
    public override string GroupName => "地图设置";
    public override uint GroupPriority => 6;

    [ModdedToggleOption("启用随机地图")]
    public bool RandomMaps { get; set; } = false;

    public ModdedNumberOption SkeldChance { get; } = new ModdedNumberOption("Skeld概率", 0, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<TownOfUsMapOptions>.Instance.RandomMaps,
    };

    public ModdedNumberOption MiraChance { get; } = new ModdedNumberOption("米拉概率", 0, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<TownOfUsMapOptions>.Instance.RandomMaps,
    };

    public ModdedNumberOption PolusChance { get; } = new ModdedNumberOption("Polus概率", 0, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<TownOfUsMapOptions>.Instance.RandomMaps,
    };

    public ModdedNumberOption AirshipChance { get; } = new ModdedNumberOption("飞艇概率", 0, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<TownOfUsMapOptions>.Instance.RandomMaps,
    };

    public ModdedNumberOption FungleChance { get; } = new ModdedNumberOption("真菌概率", 0, 0, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<TownOfUsMapOptions>.Instance.RandomMaps,
    };

    // [ModdedNumberOption("dlekS概率", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    // public float dlekSChance { get; set; }

    public ModdedNumberOption SubmergedChance { get; } = new ModdedNumberOption("水下概率", 0, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<TownOfUsMapOptions>.Instance.RandomMaps,
    };

    public ModdedNumberOption LevelImpostorChance { get; } = new ModdedNumberOption("关卡内鬼概率", 0, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<TownOfUsMapOptions>.Instance.RandomMaps,
    };

    [ModdedToggleOption("Skeld/米拉半视野模式")]
    public bool SmallMapHalfVision { get; set; } = false;

    [ModdedNumberOption("米拉HQ冷却缩减", 0f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float SmallMapDecreasedCooldown { get; set; } = 0f;

    [ModdedNumberOption("飞艇/水下冷却增加", 0f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float LargeMapIncreasedCooldown { get; set; } = 0f;

    [ModdedNumberOption("Skeld/米拉增加短任务", 0f, 5f)]
    public float SmallMapIncreasedShortTasks { get; set; } = 0f;

    [ModdedNumberOption("Skeld/米拉增加长任务", 0f, 3f)]
    public float SmallMapIncreasedLongTasks { get; set; } = 0f;

    [ModdedNumberOption("飞艇/水下减少短任务", 0f, 5f)]
    public float LargeMapDecreasedShortTasks { get; set; } = 0f;

    [ModdedNumberOption("飞艇/水下减少长任务", 0f, 3f)]
    public float LargeMapDecreasedLongTasks { get; set; } = 0f;

    // MapNames 6 is Submerged
    public float GetMapBasedCooldownDifference()
    {
        return (MapNames)GameOptionsManager.Instance.currentNormalGameOptions.MapId switch
        {
            MapNames.MiraHQ => -SmallMapDecreasedCooldown,
            MapNames.Airship or (MapNames)6 => LargeMapIncreasedCooldown,
            _ => 0,
        };
    }

    public int GetMapBasedShortTasks()
    {
        return (MapNames)GameOptionsManager.Instance.currentNormalGameOptions.MapId switch
        {
            MapNames.MiraHQ or MapNames.Skeld or MapNames.Dleks => (int)SmallMapIncreasedShortTasks,
            MapNames.Airship or (MapNames)6 => -(int)LargeMapDecreasedShortTasks,
            _ => 0,
        };
    }

    public int GetMapBasedLongTasks()
    {
        return (MapNames)GameOptionsManager.Instance.currentNormalGameOptions.MapId switch
        {
            MapNames.MiraHQ or MapNames.Skeld or MapNames.Dleks => (int)SmallMapIncreasedLongTasks,
            MapNames.Airship or (MapNames)6 => -(int)LargeMapDecreasedLongTasks,
            _ => 0,
        };
    }
}
