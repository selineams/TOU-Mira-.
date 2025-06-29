using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class AltruistOptions : AbstractOptionGroup<AltruistRole>
{
    public override string GroupName => "殉道者";

    [ModdedNumberOption("复活持续时间", 1f, 15f, 1f, MiraNumberSuffixes.Seconds)]
    public float ReviveDuration { get; set; } = 5f;

    [ModdedNumberOption("复活范围", 0.05f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float ReviveRange { get; set; } = 0.25f;

    [ModdedNumberOption("复活次数", 1f, 5f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxRevives { get; set; } = 3;

    [ModdedToggleOption("复活开始时隐藏尸体")]
    public bool HideAtBeginningOfRevive { get; set; } = false;
}
