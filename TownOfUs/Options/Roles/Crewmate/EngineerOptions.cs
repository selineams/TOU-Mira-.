using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class EngineerOptions : AbstractOptionGroup<EngineerTouRole>
{
    public override string GroupName => "工程师";

    [ModdedNumberOption("每局可用通风口次数", 0f, 30f, 5f, MiraNumberSuffixes.None, "0", zeroInfinity: true)]
    public float MaxVents { get; set; } = 15f;
    public ModdedToggleOption TaskUses { get; } = new ModdedToggleOption("完成任务可获得更多通风口次数", true)
    {
        Visible = () => OptionGroupSingleton<EngineerOptions>.Instance.MaxVents != 0,
    };

    [ModdedNumberOption("通风口使用冷却", 0f, 25f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float VentCooldown { get; set; } = 5f;

    [ModdedNumberOption("通风口使用持续时间", 0f, 25f, 5f, MiraNumberSuffixes.Seconds, zeroInfinity: true)]
    public float VentDuration { get; set; } = 25f;

    [ModdedNumberOption("每局可用修理次数", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxFixes { get; set; } = 3f;

    [ModdedNumberOption("修理延迟时间", 0f, 5f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float FixDelay { get; set; } = 1f;
}
