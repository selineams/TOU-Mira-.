using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class AmnesiacOptions : AbstractOptionGroup<AmnesiacRole>
{
    public override string GroupName => "失忆者";

    [ModdedToggleOption("失忆者获得指向尸体的箭头")]
    public bool RememberArrows { get; set; } = true;
    public ModdedNumberOption RememberArrowDelay { get; } = new ModdedNumberOption("死亡后箭头出现时间", 0f, 0f, 15f, 1f, MiraNumberSuffixes.Seconds, "0")
    {
        Visible = () => OptionGroupSingleton<AmnesiacOptions>.Instance.RememberArrows,
    };
}
