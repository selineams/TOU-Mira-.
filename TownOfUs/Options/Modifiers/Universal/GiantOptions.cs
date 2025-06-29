using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Universal;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Universal;

public sealed class GiantOptions : AbstractOptionGroup<GiantModifier>
{
    public override string GroupName => "巨人";
    public override uint GroupPriority => 25;
    public override Color GroupColor => TownOfUsColors.Giant;

    [ModdedNumberOption("巨人速度", 0.25f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float GiantSpeed { get; set; } = 0.75f;
}
