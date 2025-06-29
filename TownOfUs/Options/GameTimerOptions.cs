using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace TownOfUs.Options;

public sealed class GameTimerOptions : AbstractOptionGroup
{
    public override string GroupName => "内鬼游戏时长计时器";
    public override uint GroupPriority => 3;

    [ModdedToggleOption("内鬼游戏时长限制")]
    public bool GameTimerEnabled { get; set; } = false;
    
    public ModdedNumberOption GameTimeLimit { get; } = new ModdedNumberOption("游戏时间限制", 12f, 1f, 12f, 0.5f, MiraNumberSuffixes.None, "0.0m")
    {
        Visible = () => OptionGroupSingleton<GameTimerOptions>.Instance.GameTimerEnabled,
    };
}