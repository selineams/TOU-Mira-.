using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class JanitorOptions : AbstractOptionGroup<JanitorRole>
{
    public override string GroupName => "清理者";

    [ModdedNumberOption("每局可用清理次数", 0f, 15f, 5f, MiraNumberSuffixes.None, "0", zeroInfinity: true)]
    public float MaxClean { get; set; } = 15f;
    [ModdedNumberOption("清理冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float CleanCooldown { get; set; } = 10f;
    [ModdedNumberOption("清理延迟", 0f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float CleanDelay { get; set; } = 0f;
    [ModdedToggleOption("击杀与清理冷却同步重置")]
    public bool ResetCooldowns { get; set; } = false;
    [ModdedToggleOption("清理者可与队友一起击杀")]
    public bool JanitorKill { get; set; } = true;
}
