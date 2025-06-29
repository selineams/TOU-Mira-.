using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Neutral;

namespace TownOfUs.Options.Roles.Neutral;

public sealed class VampireOptions : AbstractOptionGroup<VampireRole>
{
    public override string GroupName => "吸血魔";

    [ModdedNumberOption("咬击冷却", 10f, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float BiteCooldown { get; set; } = 25f;

    [ModdedNumberOption("每局最大吸血魔数量（含原生、死亡的）", 2, 5, 1, MiraNumberSuffixes.None, "0")]
    public float MaxVampires { get; set; } = 4;

    [ModdedToggleOption("吸血魔拥有伪装者视野")]
    public bool HasVision { get; set; } = true;

    [ModdedToggleOption("新吸血魔可刺杀")]
    public bool CanGuessAsNewVamp { get; set; } = false;

    [ModdedEnumOption("有效转化对象", typeof(BiteOptions), ["船员", "船员+中立善良", "船员+中立邪恶", "船员+中立善良+中立邪恶", "船员+情侣", "船员+情侣+中立善良", "船员+情侣+中立邪恶", "船员+情侣+中立善良+中立邪恶"])]
    public BiteOptions ConvertOptions { get; set; } = BiteOptions.CrewLoversNeutralBenignAndNeutralEvil;

    [ModdedToggleOption("新吸血魔可转化")]
    public bool CanConvertAsNewVamp { get; set; } = true;

    [ModdedToggleOption("吸血魔可进通风口")]
    public bool CanVent { get; set; } = true;
}
public enum BiteOptions
{
    OnlyCrewmates,
    CrewAndNeutralBenign,
    CrewAndNeutralEvil,
    CrewNeutralBenignAndNeutralEvil,
    CrewAndLovers,
    CrewLoversAndNeutralBenign,
    CrewLoversAndNeutralEvil,
    CrewLoversNeutralBenignAndNeutralEvil,
}
