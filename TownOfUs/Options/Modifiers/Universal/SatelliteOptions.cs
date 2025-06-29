using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Universal;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Universal;

public sealed class SatelliteOptions : AbstractOptionGroup<SatelliteModifier>
{
    public override string GroupName => "卫星";
    public override uint GroupPriority => 27;
    public override Color GroupColor => TownOfUsColors.Satellite;

    [ModdedNumberOption("按钮冷却", 5f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float Cooldown { get; set; } = 15f;

    [ModdedNumberOption("最大可用次数", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxNumCast { get; set; } = 5f;

    [ModdedToggleOption("每回合限用一次")]
    public bool OneUsePerRound { get; set; } = true;
    
    [ModdedToggleOption("首回合可用")]
    public bool FirstRoundUse { get; set; } = true;
}
