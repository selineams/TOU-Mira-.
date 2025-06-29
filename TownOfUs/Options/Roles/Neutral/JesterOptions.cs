using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class JesterOptions : AbstractOptionGroup<JesterRole>
{
    public override string GroupName => "小丑";

    [ModdedToggleOption("小丑可召开会议")]
    public bool CanButton { get; set; } = true;

    [ModdedToggleOption("小丑可藏身通风口")]
    public bool CanVent { get; set; } = true;

    [ModdedToggleOption("小丑拥有伪装者视野")]
    public bool ImpostorVision { get; set; } = true;

    [ModdedToggleOption("小丑长时间固定活动自杀")]
    public bool ScatterOn { get; set; } = true;

    [ModdedNumberOption("小丑多久固定活动自杀", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float ScatterTimer { get; set; } = 55f;

    [ModdedEnumOption("小丑胜利效果", typeof(JestWinOptions), ["结束游戏", "胜利离差并带走一人", "继续游戏"])]
    public JestWinOptions JestWin { get; set; } = JestWinOptions.EndsGame;

}

public enum JestWinOptions
{
    EndsGame,
    Haunts,
    Nothing
}