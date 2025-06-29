using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class ImitatorOptions : AbstractOptionGroup<ImitatorRole>
{
    public override string GroupName => "效颦者";

    [ModdedToggleOption("模仿特定中立为类似船员角色")]
    public bool ImitateNeutrals { get; set; } = true;

    [ModdedToggleOption("模仿特定内鬼为类似船员角色")]
    public bool ImitateImpostors { get; set; } = true;
}
