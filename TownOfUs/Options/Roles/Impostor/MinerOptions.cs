using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class MinerOptions : AbstractOptionGroup<MinerRole>
{
    public override string GroupName => "管道工";

    [ModdedNumberOption("每局可用管道数量", 0f, 30f, 5f, MiraNumberSuffixes.None, "0", zeroInfinity: true)]
    public float MaxMines { get; set; } = 15f;

    [ModdedNumberOption("管道冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float MineCooldown { get; set; } = 10f;

    [ModdedEnumOption("管道可见性", typeof(MineVisiblityOptions), ["立即可见", "使用后可见"])]
    public MineVisiblityOptions MineVisibility { get; set; } = MineVisiblityOptions.Immediate;

    public ModdedNumberOption MineDelay { get; } = new ModdedNumberOption("管道延迟可见", 10f, 0f, 10f, 0.5f, MiraNumberSuffixes.Seconds)
    {
        Visible = () => OptionGroupSingleton<MinerOptions>.Instance.MineVisibility is MineVisiblityOptions.Immediate,
    };

    [ModdedToggleOption("管道工可与队友一起击杀")]
    public bool MinerKill { get; set; } = true;
}

public enum MineVisiblityOptions
{
    Immediate,
    AfterUse,
}
