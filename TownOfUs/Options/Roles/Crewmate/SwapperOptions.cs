using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class SwapperOptions : AbstractOptionGroup<SwapperRole>
{
    public override string GroupName => "换票师";

    [ModdedToggleOption("换票师可召开会议")]
    public bool CanButton { get; set; } = false;
}
