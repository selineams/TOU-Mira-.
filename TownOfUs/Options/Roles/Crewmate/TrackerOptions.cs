using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class TrackerOptions : AbstractOptionGroup<TrackerTouRole>
{
    public override string GroupName => "追踪者";

    [ModdedNumberOption("追踪冷却", 1f, 30f, 1f, MiraNumberSuffixes.Seconds)]
    public float TrackCooldown { get; set; } = 10f;

    [ModdedNumberOption("最大追踪次数", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxTracks { get; set; } = 5f;

    [ModdedNumberOption("箭头刷新间隔", 0.5f, 15f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float UpdateInterval { get; set; } = 2.5f;

    [ModdedToggleOption("死亡时追踪者箭头发声")]
    public bool SoundOnDeactivate { get; set; } = true;

    [ModdedToggleOption("每回合重置追踪箭头")]
    public bool ResetOnNewRound { get; set; } = true;

    public ModdedToggleOption TaskUses { get; } = new ModdedToggleOption("完成任务可获得更多追踪次数", true)
    {
        Visible = () => !OptionGroupSingleton<TrackerOptions>.Instance.ResetOnNewRound,
    };
}
