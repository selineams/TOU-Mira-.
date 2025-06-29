using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class DoomsayerOptions : AbstractOptionGroup<DoomsayerRole>
{
    public override string GroupName => "末日预言者";

    [ModdedNumberOption("洞察冷却", 1f, 30f, 1f, MiraNumberSuffixes.Seconds)]
    public float ObserveCooldown { get; set; } = 10f;

    [ModdedNumberOption("获胜所需猜测次数", 2f, 5f, 1f, MiraNumberSuffixes.None, "0")]
    public float DoomsayerGuessesToWin { get; set; } = 3f;

    [ModdedToggleOption("末日预言者一次性猜全部角色")]
    public bool DoomsayerGuessAllAtOnce { get; set; } = false;
    public ModdedToggleOption DoomsayerKillOnlyLast { get; set; } = new("仅击杀最后一名受害者", false)
    {
        Visible = () => OptionGroupSingleton<DoomsayerOptions>.Instance.DoomsayerGuessAllAtOnce
    };

    [ModdedToggleOption("末日预言者无法使用洞察")]
    public bool CantObserve { get; set; } = false;

    [ModdedEnumOption("末日预言者胜利效果", typeof(DoomWinOptions), ["结束游戏", "胜利离场", "继续游戏"])]
    public DoomWinOptions DoomWin { get; set; } = DoomWinOptions.EndsGame;

}
public enum DoomWinOptions
{
    EndsGame,
    Leaves,
    Nothing
}