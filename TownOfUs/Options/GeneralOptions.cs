using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace TownOfUs.Options;

public sealed class GeneralOptions : AbstractOptionGroup
{
    public override string GroupName => "通用设置";
    public override uint GroupPriority => 1;

    [ModdedEnumOption("角色介绍中显示的附加特性类型", typeof(ModReveal))]
    public ModReveal ModifierReveal { get; set; } = ModReveal.Faction;

    [ModdedToggleOption("隐蔽灰人通讯")]
    public bool CamouflageComms { get; set; } = true;

    [ModdedToggleOption("伪装期间可击杀任何人")]
    public bool KillDuringCamoComms { get; set; } = true;

    [ModdedToggleOption("内鬼互不知晓身份模式")]
    public bool FFAImpostorMode { get; set; } = false;
    public ModdedToggleOption ImpsKnowRoles { get; set; } = new("内鬼知晓彼此职业", true)
    {
        Visible = () => !OptionGroupSingleton<GeneralOptions>.Instance.FFAImpostorMode
    };
    public ModdedToggleOption ImpostorChat { get; set; } = new("内鬼拥有专属会议聊天", true)
    {
        Visible = () => !OptionGroupSingleton<GeneralOptions>.Instance.FFAImpostorMode
    };

    [ModdedToggleOption("吸血魔拥有专属会议聊天")]
    public bool VampireChat { get; set; } = true;

    [ModdedToggleOption("死亡者全知全能")]
    public bool TheDeadKnow { get; set; } = true;

    [ModdedNumberOption("开局冷却时间", 10f, 30f, 2.5f, MiraNumberSuffixes.Seconds, "0.#")]
    public float GameStartCd { get; set; } = 15f;

    [ModdedNumberOption("临时存档冷却重置", 0f, 15f, 0.5f, MiraNumberSuffixes.Seconds, "0.#")]
    public float TempSaveCdReset { get; set; } = 2.5f;

    [ModdedToggleOption("医疗舱可并行扫描")]
    public bool ParallelMedbay { get; set; } = true;

    [ModdedEnumOption("禁用会议跳过按钮", typeof(SkipState))]
    public SkipState SkipButtonDisable { get; set; } = SkipState.No;

    [ModdedToggleOption("首死者下局获得护盾")]
    public bool FirstDeathShield { get; set; } = true;

    [ModdedToggleOption("强力船员在场继续游戏")]
    public bool CrewKillersContinue { get; set; } = true;

    [ModdedToggleOption("视野外隐藏通风动画")]
    public bool HideVentAnimationNotInVision { get; set; } = true;
}

public enum ModReveal
{
    Alliance,
    Universal,
    Faction,
    None,
}

public enum SkipState
{
    No,
    Emergency,
    Always,
}

public enum OnTaskComplete
{
    Off,
    Sheriff,
    Veteran,
    Vigilante,
    Random,
}
