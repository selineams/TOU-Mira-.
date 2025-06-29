using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Alliance;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Alliance;

public sealed class LoversOptions : AbstractOptionGroup<LoverModifier>
{
    public override string GroupName => "恋人";
    public override uint GroupPriority => 11;
    public override Color GroupColor => TownOfUsColors.Lover;

    [ModdedToggleOption("恋人同生共死")] // Both Lovers Die And Revive Together
    public bool BothLoversDie { get; set; } = true;

    [ModdedNumberOption("内鬼恋人概率", 0, 100, 10f, MiraNumberSuffixes.Percent)] // Loving Impostor Probability
    public float LovingImpPercent { get; set; } = 30;

    [ModdedToggleOption("内鬼可以互为恋人")] // Impostors Can Be Lovers Together
    public bool ImpLovers { get; set; } = false;

    [ModdedToggleOption("中立角色可以成为恋人")] // Neutral Roles Can Be Lovers
    public bool NeutralLovers { get; set; } = true;

    [ModdedToggleOption("恋人可以杀死同阵营队友")] // Lover Can Kill Faction Teammates
    public bool LoverKillTeammates { get; set; } = true;
}
