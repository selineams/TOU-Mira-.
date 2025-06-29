using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;
using UnityEngine;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class EscapistOptions : AbstractOptionGroup<EscapistRole>
{
    public override string GroupName => "逃逸者";
    public override Color GroupColor => Palette.ImpostorRoleRed;

    [ModdedNumberOption("每局可用回溯次数", 0f, 15f, 1f, MiraNumberSuffixes.None, "0", zeroInfinity: true)]
    public float MaxEscapes { get; set; } = 15f;

    [ModdedNumberOption("回溯冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float RecallCooldown { get; set; } = 15f;

    [ModdedToggleOption("逃逸者可进通风口")]
    public bool CanVent { get; set; } = true;
}
