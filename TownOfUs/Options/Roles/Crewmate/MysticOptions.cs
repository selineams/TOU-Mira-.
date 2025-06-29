using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class MysticOptions : AbstractOptionGroup<MysticRole>
{
    public override string GroupName => "灵媒";

    [ModdedNumberOption("尸体箭头持续时间", 0f, 1f, 0.05f, MiraNumberSuffixes.Seconds, "0.00")]
    public float MysticArrowDuration { get; set; } = 1f;
}
