using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using UnityEngine;

namespace TownOfUs.Options.Modifiers;

public sealed class AllianceModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "联盟附加特性";
    public override Color GroupColor => Color.white;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 0;

    [ModdedNumberOption("营己徒概率", 0, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float EgotistChance { get; set; } = 100;
    
    [ModdedNumberOption("恋人概率", 0, 100, 10f, MiraNumberSuffixes.Percent)]
    public float LoversChance { get; set; } = 100;
}
