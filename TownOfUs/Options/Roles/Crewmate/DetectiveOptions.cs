using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class DetectiveOptions : AbstractOptionGroup<DetectiveRole>
{
    public override string GroupName => "侧写师";

    [ModdedNumberOption("侧写师调查冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ExamineCooldown { get; set; } = 10f;

    [ModdedToggleOption("显示侧写师报告")]
    public bool DetectiveReportOn { get; set; } = true;
    public ModdedNumberOption DetectiveRoleDuration { get; set; } = new("最大死亡时间 —— 侧写报告包含职业", 15f, 0f, 60f, 2.5f, MiraNumberSuffixes.Seconds)
    {
        Visible = () => !OptionGroupSingleton<DetectiveOptions>.Instance.DetectiveReportOn
    };
    public ModdedNumberOption DetectiveFactionDuration { get; set; } = new("最大死亡时间 —— 侧写报告包含阵营", 30f, 0f, 60f, 2.5f, MiraNumberSuffixes.Seconds)
    {
        Visible = () => !OptionGroupSingleton<DetectiveOptions>.Instance.DetectiveReportOn
    };
}
