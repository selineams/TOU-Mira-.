using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace TownOfUs.Options;

public sealed class RoleOptions : AbstractOptionGroup
{
    public override string GroupName => "职业设置";
    public override uint GroupPriority => 2;

    public static readonly string[] OptionStrings =
    [
        "常规 <color=#66FFFFFF>船员</color>",
        "随机 <color=#66FFFFFF>船员</color>",
        "<color=#66FFFFFF>船员</color> 侦查型",
        "<color=#66FFFFFF>船员</color> 击杀型",
        "<color=#66FFFFFF>船员</color> 防护型",
        "<color=#66FFFFFF>船员</color> 能力型",
        "<color=#66FFFFFF>船员</color> 支援型",
        "特殊 <color=#66FFFFFF>船员</color>",
        "非<color=#FF0000FF>内鬼</color>",
        "常规 <color=#999999FF>中立</color>",
        "随机 <color=#999999FF>中立</color>",
        "<color=#999999FF>中立</color> 善良型",
        "<color=#999999FF>中立</color> 邪恶型",
        "<color=#999999FF>中立</color> 杀手型",
        "常规 <color=#FF0000FF>内鬼</color>",
        "随机 <color=#FF0000FF>内鬼</color>",
        "<color=#FF0000FF>内鬼</color> 型",
        "<color=#FF0000FF>内鬼</color> 击杀型",
        "<color=#FF0000FF>内鬼</color> 支援型",
        "任意"
    ];

    [ModdedToggleOption("减少内鬼连续概率")]
    public bool LastImpostorBias { get; set; } = true;

    public ModdedNumberOption ImpostorBiasPercent { get; } = new("减少概率", 50f, 0f, 100f, 5f, MiraNumberSuffixes.Percent)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.LastImpostorBias
    };

    [ModdedToggleOption("启用职业列表")]
    public bool RoleListEnabled { get; set; } = true;

    public ModdedEnumOption Slot1 { get; } = new("槽位1", (int)RoleListOption.CrewCommon, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot2 { get; } = new("槽位2", (int)RoleListOption.CrewCommon, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot3 { get; } = new("槽位3", (int)RoleListOption.NeutCommon, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot4 { get; } = new("槽位4", (int)RoleListOption.ImpCommon, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot5 { get; } = new("槽位5", (int)RoleListOption.CrewCommon, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot6 { get; } = new("槽位6", (int)RoleListOption.NeutKilling, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot7 { get; } = new("槽位7", (int)RoleListOption.CrewCommon, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot8 { get; } = new("槽位8", (int)RoleListOption.ImpRandom, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot9 { get; } = new("槽位9", (int)RoleListOption.ImpCommon, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot10 { get; } = new("槽位10", (int)RoleListOption.NeutCommon, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot11 { get; } = new("槽位11", (int)RoleListOption.CrewCommon, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot12 { get; } = new("槽位12", (int)RoleListOption.NeutKilling, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot13 { get; } = new("槽位13", (int)RoleListOption.CrewCommon, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot14 { get; } = new("槽位14", (int)RoleListOption.ImpRandom, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedEnumOption Slot15 { get; } = new("槽位15", (int)RoleListOption.NeutCommon, typeof(RoleListOption), OptionStrings)
    {
        Visible = () => OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedNumberOption MinNeutralBenign { get; } = new("最小中立善良", 1f, 0f, 3f, 1f, MiraNumberSuffixes.None, "0")
    {
        Visible = () => !OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedNumberOption MaxNeutralBenign { get; } = new("最大中立善良", 2f, 0f, 3f, 1f, MiraNumberSuffixes.None, "0")
    {
        Visible = () => !OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedNumberOption MinNeutralEvil { get; } = new("最小中立邪恶", 1f, 0f, 3f, 1f, MiraNumberSuffixes.None, "0")
    {
        Visible = () => !OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedNumberOption MaxNeutralEvil { get; } = new("最大中立邪恶", 2f, 0f, 3f, 1f, MiraNumberSuffixes.None, "0")
    {
        Visible = () => !OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedNumberOption MinNeutralKiller { get; } = new("最小中立杀手", 2f, 0f, 5f, 1f, MiraNumberSuffixes.None, "0")
    {
        Visible = () => !OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };

    public ModdedNumberOption MaxNeutralKiller { get; } = new("最大中立杀手", 2f, 0f, 5f, 1f, MiraNumberSuffixes.None, "0")
    {
        Visible = () => !OptionGroupSingleton<RoleOptions>.Instance.RoleListEnabled
    };
}

public enum RoleListOption
{
    CrewCommon,      // 常规船员
    CrewRandom,      // 随机船员
    CrewInvest,      // 侦查船员
    CrewKilling,     // 击杀船员
    CrewProtective,  // 防护船员
    CrewPower,       // 能力船员
    CrewSupport,     // 支援船员
    CrewSpecial,     // 特殊船员
    NonImp,          // 非内鬼
    NeutCommon,      // 常规中立
    NeutRandom,      // 随机中立
    NeutBenign,      // 中立善良
    NeutEvil,        // 中立邪恶
    NeutKilling,     // 中立杀手
    ImpCommon,       // 常规内鬼
    ImpRandom,       // 随机内鬼
    ImpConceal,      // 隐藏内鬼
    ImpKilling,      // 击杀内鬼
    ImpSupport,      // 支援内鬼
    Any              // 任意
}