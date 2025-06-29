using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Crewmate;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Crewmate;

public sealed class NoisemakerOptions : AbstractOptionGroup<NoisemakerModifier>
{
    public override string GroupName => "大嗓门";
    public override uint GroupPriority => 34;
    public override Color GroupColor => TownOfUsColors.Noisemaker;

    [ModdedToggleOption("内鬼收到大嗓门警报")]
    public bool ImpostorsAlerted { get; set; } = true;

    [ModdedToggleOption("中立杀手收到大嗓门警报")]
    public bool NeutsAlerted { get; set; } = true;

    [ModdedToggleOption("通讯破坏阻止大嗓门警报")]
    public bool CommsAffected { get; set; } = false;

    [ModdedToggleOption("大嗓门仅在有尸体时触发")]
    public bool BodyCheck { get; set; } = true;

    [ModdedNumberOption("大嗓门警报持续时间", 1f, 20f, 1f, MiraNumberSuffixes.Seconds)]
    public float AlertDuration { get; set; } = 5f;
}
