using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Universal;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Universal;

public sealed class ButtonBarryOptions : AbstractOptionGroup<ButtonBarryModifier>
{
    public override string GroupName => "执钮人";
    public override uint GroupPriority => 22;
    public override Color GroupColor => TownOfUsColors.ButtonBarry;

    [ModdedNumberOption("按钮冷却", 2.5f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float Cooldown { get; set; } = 30f;

    [ModdedNumberOption("最大可用次数", 1f, 3f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxNumButtons { get; set; } = 1f;

    [ModdedToggleOption("无视破坏")]
    public bool IgnoreSabo { get; set; } = true;
    
    [ModdedToggleOption("首回合可用")]
    public bool FirstRoundUse { get; set; } = false;
}
