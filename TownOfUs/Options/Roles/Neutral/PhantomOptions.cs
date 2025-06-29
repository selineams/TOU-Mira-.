using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class PhantomOptions : AbstractOptionGroup<PhantomTouRole>
{
    public override string GroupName => "幻影";

    [ModdedNumberOption("可点击前剩余任务数", 1, 15, 1)]
    public float NumTasksLeftBeforeClickable { get; set; } = 1f;

    [ModdedEnumOption("幻影胜利效果", typeof(PhantomWinOptions), ["结束游戏", "胜利离场并带走一人", "继续游戏"])]
    public PhantomWinOptions PhantomWin { get; set; } = PhantomWinOptions.Nothing;

}
public enum PhantomWinOptions
{
    EndsGame,
    Spooks,
    Nothing
}