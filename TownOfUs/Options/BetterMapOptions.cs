using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;

namespace TownOfUs.Options;

public sealed class BetterMapOptions : AbstractOptionGroup
{
    public override string GroupName => "优化Polus地图";
    public override uint GroupPriority => 5;
    public override Func<bool> GroupVisible => () =>
        (GameOptionsManager.Instance.currentGameOptions.MapId == (int)ShipStatus.MapType.Pb) || (OptionGroupSingleton<TownOfUsMapOptions>.Instance.RandomMaps && OptionGroupSingleton<TownOfUsMapOptions>.Instance.PolusChance > 0);

    [ModdedToggleOption("优化Polus通风网络")]
    public bool BPVentNetwork { get; set; } = false;

    [ModdedToggleOption("Polus：生命体征移至实验室")]
    public bool BPVitalsInLab { get; set; } = false;

    [ModdedToggleOption("Polus：低温移至死亡谷")]
    public bool BPTempInDeathValley { get; set; } = false;

    [ModdedToggleOption("Polus：重启WiFi与航线任务互换")]
    public bool BPSwapWifiAndChart { get; set; } = false;
    
    [ModdedToggleOption("飞艇：飞艇门为Polus门样式")]
    public bool AirshipPolusDoors { get; set; } = false;
}