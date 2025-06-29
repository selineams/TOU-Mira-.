using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class GlitchOptions : AbstractOptionGroup<GlitchRole>
{
    public override string GroupName => "混沌";

    [ModdedNumberOption("混沌击杀冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 20f;

    [ModdedNumberOption("混沌模仿冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float MimicCooldown { get; set; } = 10f;

    [ModdedNumberOption("混沌模仿持续时间", 5f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float MimicDuration { get; set; } = 15f;
    /* [ModdedToggleOption("使用模仿菜单时可移动")]
    public bool MoveWithMenu { get; set; } = true; */
    [ModdedNumberOption("黑入冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float HackCooldown { get; set; } = 10f;

    [ModdedNumberOption("黑入持续时间", 5f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float HackDuration { get; set; } = 15f;

    [ModdedToggleOption("混沌可进通风口")]
    public bool CanVent { get; set; } = true;

}
