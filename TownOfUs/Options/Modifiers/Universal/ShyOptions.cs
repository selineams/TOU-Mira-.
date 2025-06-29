using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Universal;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Universal;

public sealed class ShyOptions : AbstractOptionGroup<ShyModifier>
{
    public override string GroupName => "变色龙";
    public override uint GroupPriority => 28;
    public override Color GroupColor => TownOfUsColors.Shy;

    [ModdedNumberOption("变透明延迟", 1f, 15f, 1f, MiraNumberSuffixes.Seconds)]
    public float InvisDelay { get; set; } = 2f;

    [ModdedNumberOption("变透明持续时间", 1f, 15f, 1f, MiraNumberSuffixes.Seconds)]
    public float TransformInvisDuration { get; set; } = 3f;

    [ModdedNumberOption("最终透明度", 0f, 80f, 10f, MiraNumberSuffixes.Percent)]
    public float FinalTransparency { get; set; } = 0f;
}
