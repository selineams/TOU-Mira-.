using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Impostor;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Impostor;

public sealed class SaboteurOptions : AbstractOptionGroup<SaboteurModifier>
{
    public override string GroupName => "破坏者";
    public override Color GroupColor => Palette.ImpostorRoleHeaderRed;
    public override uint GroupPriority => 41;

    [ModdedNumberOption("破坏冷却缩减", 5f, 15f, 1f, MiraNumberSuffixes.Seconds, "0")]
    public float ReducedSaboCooldown { get; set; } = 15f;
}
