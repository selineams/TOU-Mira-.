using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Impostor;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Impostor;

public sealed class UnderdogOptions : AbstractOptionGroup<UnderdogModifier>
{
    public override string GroupName => "潜伏者";
    public override Color GroupColor => Palette.ImpostorRoleHeaderRed;
    public override uint GroupPriority => 43;

    [ModdedNumberOption("击杀冷却加成", 2.5f, 10f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldownIncrease { get; set; } = 10f;

    [ModdedToggleOption("有2名及以上内鬼时增加击杀冷却")]
    public bool ExtraImpsKillCooldown { get; set; } = false;
}
