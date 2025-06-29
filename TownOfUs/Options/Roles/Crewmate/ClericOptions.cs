using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class ClericOptions : AbstractOptionGroup<ClericRole>
{
    public override string GroupName => "牧师";

    [ModdedNumberOption("屏障冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float BarrierCooldown { get; set; } = 25f;
    [ModdedNumberOption("屏障持续时间", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float BarrierDuration { get; set; } = 25f;
    [ModdedNumberOption("净化冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float CleanseCooldown { get; set; } = 25f;

    [ModdedEnumOption("屏障显示对象", typeof(BarrierOptions), ["被结界者", "牧师", "被结界者+牧师"])]
    public BarrierOptions ShowBarriered { get; set; } = BarrierOptions.SelfAndCleric;

    [ModdedToggleOption("牧师收到攻击通知")]
    public bool AttackNotif { get; set; } = true;
}

public enum BarrierOptions
{
    Self,
    Cleric,
    SelfAndCleric,
}
