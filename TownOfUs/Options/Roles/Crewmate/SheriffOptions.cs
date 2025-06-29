using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class SheriffOptions : AbstractOptionGroup<SheriffRole>
{
    public override string GroupName => "警长";

    [ModdedNumberOption("警长击杀冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 25f;

    [ModdedToggleOption("警长可自报尸体")]
    public bool SheriffBodyReport { get; set; } = false;
    [ModdedToggleOption("警长首回合可开枪")]
    public bool FirstRoundUse { get; set; } = true;

    [ModdedToggleOption("警长可射击中立邪恶职业")]
    public bool ShootNeutralEvil { get; set; } = true;

    [ModdedToggleOption("警长可射击中立杀手职业")]
    public bool ShootNeutralKiller { get; set; } = true;

    [ModdedEnumOption("警长误杀后死亡者", typeof(MisfireOptions), ["警长", "目标", "警长和目标", "无"])]
    public MisfireOptions MisfireType { get; set; } = MisfireOptions.Sheriff;

}
public enum MisfireOptions
{
    Sheriff,
    Target,
    Both,
    Nobody
}
