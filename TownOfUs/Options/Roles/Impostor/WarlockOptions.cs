using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Impostor;

namespace TownOfUs.Options.Roles.Impostor;

public sealed class WarlockOptions : AbstractOptionGroup<WarlockRole>
{
    public override string GroupName => "巫术师";

    [ModdedNumberOption("击杀键充能至100%额外所需用时", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ChargeTimeDuration { get; set; } = 15f;
    [ModdedNumberOption("每次击杀增加下次充能时间增加倍率", 0f, 0.5f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float AddedTimeDuration { get; set; } = 0.05f;

    [ModdedNumberOption("满能后击杀键最长可持续使用时间", 0.05f, 5f, 0.05f, MiraNumberSuffixes.Seconds, "0.00")]
    public float DischargeTimeDuration { get; set; } = 1.5f;
}
