using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class TraitorOptions : AbstractOptionGroup<TraitorRole>
{
    public override string GroupName => "背叛者";

    [ModdedNumberOption("背叛者可生成时最少存活人数", 3f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float LatestSpawn { get; set; } = 7f;

    [ModdedToggleOption("有中立杀手存活时不生成背叛者")]
    public bool NeutralKillingStopsTraitor { get; set; } = false;

    [ModdedToggleOption("禁用本场游戏伪装者职业")]
    public bool RemoveExistingRoles { get; set; } = false;
}
