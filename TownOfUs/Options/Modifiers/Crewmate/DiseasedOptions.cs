using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Crewmate;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Crewmate;

public sealed class DiseasedOptions : AbstractOptionGroup<DiseasedModifier>
{
    public override string GroupName => "病人";
    public override uint GroupPriority => 32;
    public override Color GroupColor => TownOfUsColors.Diseased;

    [ModdedNumberOption("病人击杀冷却倍率", 1.5f, 5f, 0.5f, MiraNumberSuffixes.Multiplier)]
    public float CooldownMultiplier { get; set; } = 2f;
}
