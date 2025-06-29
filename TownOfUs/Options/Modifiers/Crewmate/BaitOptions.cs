using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Crewmate;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Crewmate;

public sealed class BaitOptions : AbstractOptionGroup<BaitModifier>
{
    public override string GroupName => "诱饵";
    public override uint GroupPriority => 31;
    public override Color GroupColor => TownOfUsColors.Bait;

    [ModdedNumberOption("最小诱饵报告延迟", 0f, 15f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float MinDelay { get; set; } = 0f;

    [ModdedNumberOption("最大诱饵报告延迟", 0f, 15f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float MaxDelay { get; set; } = 1f;
}
