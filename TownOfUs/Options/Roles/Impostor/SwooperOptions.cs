using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;
namespace TownOfUs.Options.Roles.Impostor;

public sealed class SwooperOptions : AbstractOptionGroup<SwooperRole>
{
    public override string GroupName => "隐身人";

    [ModdedNumberOption("每回合可隐身次数", 0f, 10f, 1f, MiraNumberSuffixes.None, "0", zeroInfinity: true)]
    public float MaxSwoops { get; set; } = 10f;

    [ModdedNumberOption("隐身冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float SwoopCooldown { get; set; } = 25f;

    [ModdedNumberOption("隐身持续时间", 5f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float SwoopDuration { get; set; } = 15f;

    [ModdedToggleOption("隐身人可进通风口")]
    public bool CanVent { get; set; } = true;
}
