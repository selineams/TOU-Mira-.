using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class PoliticianOptions : AbstractOptionGroup<PoliticianRole>
{
    public override string GroupName => "政治家";

    [ModdedNumberOption("竞选冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float CampaignCooldown { get; set; } = 10f;
    [ModdedToggleOption("揭示失败后禁止竞选")]
    public bool PreventCampaign { get; set; } = false;
}
