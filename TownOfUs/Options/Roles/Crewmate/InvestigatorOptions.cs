using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class InvestigatorOptions : AbstractOptionGroup<InvestigatorRole>
{
    public override string GroupName => "调查员";

    [ModdedNumberOption("足迹大小", 1f, 10f, suffixType: MiraNumberSuffixes.Multiplier)]
    public float FootprintSize { get; set; } = 4f;

    [ModdedNumberOption("足迹间隔", 0.5f, 6f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float FootprintInterval { get; set; } = 1;

    [ModdedNumberOption("足迹持续时间", 1f, 15f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float FootprintDuration { get; set; } = 10f;

    [ModdedToggleOption("匿名足迹")]
    public bool ShowAnonymousFootprints { get; set; } = false;

    [ModdedToggleOption("通风口足迹可见")]
    public bool ShowFootprintVent { get; set; } = true;
}
