using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class MedicOptions : AbstractOptionGroup<MedicRole>
{
    public override string GroupName => "法医";

    [ModdedEnumOption("可看见被保护玩家的人是", typeof(MedicOption), ["法医", "被保护人", "被保护人 + 法医", "所有人", "无"])]
    public MedicOption ShowShielded { get; set; } = MedicOption.Everyone;

    [ModdedEnumOption("可看见击杀尝试的人是", typeof(MedicOption), ["法医", "被保护人", "被保护人 + 法医", "所有人", "无"])]
    public MedicOption WhoGetsNotification { get; set; } = MedicOption.Medic;

    [ModdedToggleOption("允许法医下回合更换保护对象")]
    public bool ChangeTarget { get; set; } = true;

    [ModdedToggleOption("谋杀尝试时护盾破裂")]
    public bool ShieldBreaks { get; set; } = true;

    [ModdedToggleOption("显示法医报告")]
    public bool ShowReports { get; set; } = true;

    public ModdedNumberOption MedicReportNameDuration { get; } = new ModdedNumberOption("显示名字时间", 15f, 0f, 60f, 2.5f, MiraNumberSuffixes.Seconds)
    {
        Visible = () => OptionGroupSingleton<MedicOptions>.Instance.ShowReports,
    };

    public ModdedNumberOption MedicReportColorDuration { get; } = new ModdedNumberOption("显示颜色类型时间", 45, 0f, 60f, 2.5f, MiraNumberSuffixes.Seconds)
    {
        Visible = () => OptionGroupSingleton<MedicOptions>.Instance.ShowReports,
    };
}

public enum MedicOption
{
    Medic,
    Shielded,
    ShieldedAndMedic,
    Everyone,
    Nobody,
}
