using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Impostor;
using UnityEngine;

namespace TownOfUs.Options.Modifiers.Impostor;

public sealed class TelepathOptions : AbstractOptionGroup<TelepathModifier>
{
    public override string GroupName => "通感者";
    public override Color GroupColor => Palette.ImpostorRoleHeaderRed;
    public override uint GroupPriority => 42;

    [ModdedToggleOption("知晓队友击杀位置")]
    public bool KnowKillLocation { get; set; } = true;
    [ModdedToggleOption("知晓队友死亡时间")]
    public bool KnowDeath { get; set; } = true;

    public ModdedToggleOption KnowDeathLocation { get; } = new ModdedToggleOption("知晓队友死亡位置", true)
    {
        Visible = () => OptionGroupSingleton<TelepathOptions>.Instance.KnowDeath,
    };

    public ModdedNumberOption TelepathArrowDuration { get; } = new ModdedNumberOption("尸体箭头持续时间", 5f, 0f, 5f, 0.5f, MiraNumberSuffixes.Seconds, "0.00")
    {
        Visible = () => OptionGroupSingleton<TelepathOptions>.Instance.KnowKillLocation || (OptionGroupSingleton<TelepathOptions>.Instance.KnowDeath && OptionGroupSingleton<TelepathOptions>.Instance.KnowDeathLocation),
    };

    [ModdedToggleOption("知晓队友猜测成功")]
    public bool KnowCorrectGuess { get; set; } = true;

    [ModdedToggleOption("知晓队友猜测失败")]
    public bool KnowFailedGuess { get; set; } = true;
}
