using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class SeerOptions : AbstractOptionGroup<SeerRole>
{
    public override string GroupName => "预言家";

    [ModdedNumberOption("预言家冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float SeerCooldown { get; set; } = 25f;

    [ModdedToggleOption("船员击杀职业显示为红色")]
    public bool ShowCrewmateKillingAsRed { get; set; } = true;

    [ModdedToggleOption("中立善良职业显示为红色")]
    public bool ShowNeutralBenignAsRed { get; set; } = false;

    [ModdedToggleOption("中立邪恶职业显示为红色")]
    public bool ShowNeutralEvilAsRed { get; set; } = false;

    [ModdedToggleOption("中立杀手职业显示为红色")]
    public bool ShowNeutralKillingAsRed { get; set; } = true;

    [ModdedToggleOption("背叛者转换颜色")]
    public bool SwapTraitorColors { get; set; } = false;
}
