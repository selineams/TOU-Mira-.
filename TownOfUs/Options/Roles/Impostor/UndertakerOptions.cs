using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;
public sealed class UndertakerOptions : AbstractOptionGroup<UndertakerRole>
{
    public override string GroupName => "送葬者";

    [ModdedNumberOption("拖拽冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float DragCooldown { get; set; } = 10f;

    [ModdedNumberOption("拖拽速度倍率", 0.25f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float DragSpeedMultiplier { get; set; } = 1f;

    [ModdedToggleOption("拖拽速度受尸体大小影响")]
    public bool AffectedSpeed { get; set; } = false;

    [ModdedToggleOption("送葬者可进通风口")]
    public bool CanVent { get; set; } = true;

    public ModdedToggleOption CanVentWithBody { get; } = new ModdedToggleOption("可带尸体进通风口", true)
    {
        Visible = () => OptionGroupSingleton<UndertakerOptions>.Instance.CanVent,
    };
    [ModdedToggleOption("送葬者可与队友一起击杀")]
    public bool UndertakerKill { get; set; } = true;
}
