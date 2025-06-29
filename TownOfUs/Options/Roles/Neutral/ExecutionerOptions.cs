using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class ExecutionerOptions : AbstractOptionGroup<ExecutionerRole>
{
    public override string GroupName => "行刑者";

    [ModdedEnumOption("目标死亡后，行刑者变为", typeof(BecomeOptions))]
    public BecomeOptions OnTargetDeath { get; set; } = BecomeOptions.Mercenary;

    [ModdedToggleOption("行刑者可召开会议")]
    public bool CanButton { get; set; } = true;
    
    [ModdedEnumOption("行刑者胜利效果", typeof(ExeWinOptions), ["结束游戏", "胜利离场并带走一人", "继续游戏"])]
    public ExeWinOptions ExeWin { get; set; } = ExeWinOptions.EndsGame;

}
public enum ExeWinOptions
{
    EndsGame,
    Torments,
    Nothing
}
