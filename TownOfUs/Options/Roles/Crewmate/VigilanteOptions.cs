using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class VigilanteOptions : AbstractOptionGroup<VigilanteRole>
{
    public override string GroupName => "侠客";

    [ModdedNumberOption("侠客可击杀次数", 1f, 15f)]
    public float VigilanteKills { get; set; } = 5f;

    [ModdedToggleOption("侠客每次会议可多次击杀")]
    public bool VigilanteMultiKill { get; set; } = true;

    [ModdedToggleOption("侠客可猜中立善良职业")]
    public bool VigilanteGuessNeutralBenign { get; set; } = false;

    [ModdedToggleOption("侠客可猜中立邪恶职业")]
    public bool VigilanteGuessNeutralEvil { get; set; } = true;

    [ModdedToggleOption("侠客可猜中立杀手职业")]
    public bool VigilanteGuessNeutralKilling { get; set; } = true;

    [ModdedToggleOption("侠客可猜杀手附加特性")]
    public bool VigilanteGuessKillerMods { get; set; } = false;

    [ModdedToggleOption("侠客可猜联盟关系")]
    public bool VigilanteGuessAlliances { get; set; } = false;
    [ModdedNumberOption("侠客可猜测次数", 0f, 3f, 1f, MiraNumberSuffixes.None, "0")]
    public float MultiShots { get; set; } = 3;
}
