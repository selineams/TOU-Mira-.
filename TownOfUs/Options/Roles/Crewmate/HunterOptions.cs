using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class HunterOptions : AbstractOptionGroup<HunterRole>
{
    public override string GroupName => "巡猎者";

    [ModdedNumberOption("巡猎者击杀冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float HunterKillCooldown { get; set; } = 25f;

    [ModdedNumberOption("巡猎者盯梢冷却", 1f, 30f, 1f, MiraNumberSuffixes.Seconds)]
    public float HunterStalkCooldown { get; set; } = 10f;

    [ModdedNumberOption("巡猎者盯梢持续时间", 5f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float HunterStalkDuration { get; set; } = 25f;

    [ModdedNumberOption("最大盯梢次数", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float StalkUses { get; set; } = 5;
    [ModdedToggleOption("完成任务可获得更多追踪次数")]
    public bool TaskUses { get; set; } = true;

    [ModdedToggleOption("被投票出局时猎手击杀最后投票者")]
    public bool RetributionOnVote { get; set; } = true;

    [ModdedToggleOption("巡猎者可报告其击杀对象")]
    public bool HunterBodyReport { get; set; } = false;
}
