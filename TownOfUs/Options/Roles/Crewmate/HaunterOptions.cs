using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class HaunterOptions : AbstractOptionGroup<HaunterRole>
{
    public override string GroupName => "冤魂";

    [ModdedNumberOption("可点击前剩余任务数", 0f, 5)]
    public float NumTasksLeftBeforeClickable { get; set; } = 3f;

    [ModdedNumberOption("警告前剩余任务数", 0f, 15)]
    public float NumTasksLeftBeforeAlerted { get; set; } = 1f;

    [ModdedToggleOption("揭示中立角色")]
    public bool RevealNeutralRoles { get; set; } = true;

    [ModdedEnumOption("可被点击者", typeof(HaunterRoleClickableType), ["所有人", "非船员", "仅伪装者"])]
    public HaunterRoleClickableType HaunterCanBeClickedBy { get; set; } = HaunterRoleClickableType.NonCrew;
}

public enum HaunterRoleClickableType
{
    Everyone, 
    NonCrew,  
    ImpsOnly, 
}
