using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class GrenadierOptions : AbstractOptionGroup<GrenadierRole>
{
    public override string GroupName => "掷弹兵";

    [ModdedNumberOption("每局可用闪光弹次数", 0f, 15f, 1f, MiraNumberSuffixes.None, "0", zeroInfinity: true)]
    public float MaxFlashes { get; set; } = 15f;

    [ModdedNumberOption("闪光弹冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float GrenadeCooldown { get; set; } = 20f;

    [ModdedNumberOption("闪光弹持续时间", 5f, 15f, 1f, MiraNumberSuffixes.Seconds)]
    public float GrenadeDuration { get; set; } = 10f;

    [ModdedNumberOption("闪光半径", 0.25f, 5f, 0.25f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float FlashRadius { get; set; } = 3f;

    [ModdedToggleOption("掷弹兵可进通风口")]
    public bool CanVent { get; set; } = true;
}
