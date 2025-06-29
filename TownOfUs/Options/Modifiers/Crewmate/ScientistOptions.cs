using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Crewmate;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Crewmate;

public sealed class ScientistOptions : AbstractOptionGroup<ScientistModifier>
{
    public override string GroupName => "科学家";
    public override uint GroupPriority => 37;
    public override Color GroupColor => TownOfUsColors.Scientist;

    [ModdedToggleOption("使用生命体征时可移动")]
    public bool MoveWithMenu { get; set; } = true;

    [ModdedNumberOption("初始电量", 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float StartingCharge { get; set; } = 30f;

    [ModdedNumberOption("每回合充电量", 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float RoundCharge { get; set; } = 15f;

    [ModdedNumberOption("每任务充电量", 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float TaskCharge { get; set; } = 15f;

    [ModdedNumberOption("生命体征显示冷却", 0f, 30f, 5f, MiraNumberSuffixes.Seconds)]
    public float DisplayCooldown { get; set; } = 5f;
    
    [ModdedNumberOption("最大生命体征显示时长", 0f, 30f, 5f, MiraNumberSuffixes.Seconds, zeroInfinity: true)]
    public float DisplayDuration { get; set; } = 30f;
}
