using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Crewmate;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Crewmate;

public sealed class RottingOptions : AbstractOptionGroup<RottingModifier>
{
    public override string GroupName => "腐烂";
    public override uint GroupPriority => 36;
    public override Color GroupColor => TownOfUsColors.Rotting;

    [ModdedNumberOption("尸体腐烂前的时间", 0f, 25f, 1f, MiraNumberSuffixes.Seconds)]
    public float RotDelay { get; set; } = 5f;
}
