using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Crewmate;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Crewmate;

public sealed class OperativeOptions : AbstractOptionGroup<OperativeModifier>
{
    public override string GroupName => "监控员";
    public override uint GroupPriority => 35;
    public override Color GroupColor => new(0.8f, 0.33f, 0.37f, 1f);
    // THESE BREAK THE CAMERA MINIGAME!!
/* 
        [ModdedToggleOption("Move While Using Cameras")]
        public bool MoveWithCams { get; set; } = false;

        [ModdedToggleOption("Move While Using Fungle Binoculars")]
        public bool MoveOnFungle { get; set; } = false;
     */
    [ModdedToggleOption("使用Mira门禁时可移动")]
    public bool MoveOnMira { get; set; } = true;

    [ModdedNumberOption("初始电量", 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float StartingCharge { get; set; } = 30f;

    [ModdedNumberOption("每回合充电量", 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float RoundCharge { get; set; } = 15f;

    [ModdedNumberOption("每任务充电量", 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float TaskCharge { get; set; } = 15f;

    [ModdedNumberOption("监控显示冷却", 0f, 30f, 5f, MiraNumberSuffixes.Seconds)]
    public float DisplayCooldown { get; set; } = 5f;
    
    [ModdedNumberOption("最大监控显示时长", 0f, 30f, 5f, MiraNumberSuffixes.Seconds, zeroInfinity: true)]
    public float DisplayDuration { get; set; } = 30f;
}
