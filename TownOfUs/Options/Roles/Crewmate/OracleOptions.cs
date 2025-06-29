using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class OracleOptions : AbstractOptionGroup<OracleRole>
{
    public override string GroupName => "神谕者";

    [ModdedNumberOption("忏悔冷却", 1f, 30f, 1f, MiraNumberSuffixes.Seconds)]
    public float ConfessCooldown { get; set; } = 15f;

    [ModdedNumberOption("佑护冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float BlessCooldown { get; set; } = 15f;

    [ModdedNumberOption("揭示准确率", 0f, 100f, suffixType: MiraNumberSuffixes.Percent)]
    public float RevealAccuracyPercentage { get; set; } = 80f;

    [ModdedToggleOption("中立善良显示为邪恶")]
    public bool ShowNeutralBenignAsEvil { get; set; } = false;

    [ModdedToggleOption("中立邪恶显示为邪恶")]
    public bool ShowNeutralEvilAsEvil { get; set; } = false;

    [ModdedToggleOption("中立杀手显示为邪恶")]
    public bool ShowNeutralKillingAsEvil { get; set; } = true;
}
