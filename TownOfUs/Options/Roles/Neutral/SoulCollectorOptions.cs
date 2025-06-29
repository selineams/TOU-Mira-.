using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class SoulCollectorOptions : AbstractOptionGroup<SoulCollectorRole>
{
    public override string GroupName => "噬魂兽";

    [ModdedNumberOption("收割冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 20f;

    [ModdedToggleOption("噬魂兽可进通风口")]
    public bool CanVent { get; set; } = true;
}
