using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class ArsonistOptions : AbstractOptionGroup<ArsonistRole>
{
    public override string GroupName => "纵火狂";

    [ModdedNumberOption("点燃冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float DouseCooldown { get; set; } = 25f;

    [ModdedToggleOption("通过互动点燃")]
    public bool DouseInteractions { get; set; } = true;

    [ModdedToggleOption("经典纵火狂（无半径范围点燃）")]
    public bool LegacyArsonist { get; set; } = false;
    public ModdedNumberOption IgniteRadius { get; set; } = new("点燃半径", 1f, 0.05f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")
    {
        Visible = () => !OptionGroupSingleton<ArsonistOptions>.Instance.LegacyArsonist
    };

    [ModdedToggleOption("纵火狂可进通风口")]
    public bool CanVent { get; set; } = true;
}
