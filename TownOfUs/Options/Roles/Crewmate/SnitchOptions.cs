using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class SnitchOptions : AbstractOptionGroup<SnitchRole>
{
    public override string GroupName => "告密者";

    [ModdedToggleOption("告密者揭示中立杀手")]
    public bool SnitchNeutralRoles { get; set; } = true;

    [ModdedToggleOption("告密者可见背叛者")]
    public bool SnitchSeesTraitor { get; set; } = false;

    [ModdedToggleOption("会议中可见伪装者")]
    public bool SnitchSeesImpostorsMeetings { get; set; } = true;
    
    [ModdedToggleOption("可见被揭示玩家的角色")]
    public bool SnitchSeesRoles { get; set; } = true;

    [ModdedNumberOption("被揭示时剩余任务数", 1, 3, 1)]
    public float TaskRemainingWhenRevealed { get; set; } = 1;
}
