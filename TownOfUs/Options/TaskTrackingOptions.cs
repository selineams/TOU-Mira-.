using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace TownOfUs.Options;

public sealed class TaskTrackingOptions : AbstractOptionGroup
{
    public override string GroupName => "任务追踪";
    public override uint GroupPriority => 4;

    [ModdedToggleOption("回合中显示任务进度")]
    public bool ShowTaskRound { get; set; } = false;

    [ModdedToggleOption("会议中显示任务进度")]
    public bool ShowTaskInMeetings { get; set; } = true;

    [ModdedToggleOption("死亡后显示任务进度")]
    public bool ShowTaskDead { get; set; } = true;
}