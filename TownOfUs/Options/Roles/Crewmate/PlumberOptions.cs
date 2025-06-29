using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class PlumberOptions : AbstractOptionGroup<PlumberRole>
{
    public override string GroupName => "水管工";

    [ModdedNumberOption("冲洗冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float FlushCooldown { get; set; } = 10f;
    [ModdedNumberOption("封锁冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float BlockCooldown { get; set; } = 15f;

    [ModdedNumberOption("最大路障数量", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxBarricades { get; set; } = 5f;

    [ModdedToggleOption("完成任务可获得更多路障")]
    public bool TaskUses { get; set; } = true;
}
