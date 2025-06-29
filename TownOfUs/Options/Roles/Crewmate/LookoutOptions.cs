using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class LookoutOptions : AbstractOptionGroup<LookoutRole>
{
    public override string GroupName => "观测者";

    [ModdedNumberOption("观测冷却", 1f, 30f, 1f, MiraNumberSuffixes.Seconds)]
    public float WatchCooldown { get; set; } = 20f;

    [ModdedNumberOption("可观测玩家数上限", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxWatches { get; set; } = 5;

    [ModdedToggleOption("每回合重置观测次数")]
    public bool LoResetOnNewRound { get; set; } = true;

    public ModdedToggleOption TaskUses { get; } = new ModdedToggleOption("完成任务可获得更多观测次数", true)
    {
        Visible = () => !OptionGroupSingleton<LookoutOptions>.Instance.LoResetOnNewRound,
    };
}
