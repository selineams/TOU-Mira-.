using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class MediumOptions : AbstractOptionGroup<MediumRole>
{
    public override string GroupName => "招魂师";

    [ModdedNumberOption("招魂冷却", 0, 60f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float MediateCooldown { get; set; } = 5;

    [ModdedToggleOption("显示招魂对象外观")]
    public bool RevealMediateAppearance { get; set; } = true;

    [ModdedEnumOption("箭头可见性", typeof(MediumVisibility), ["招魂师", "被招魂者", "招魂师+被招魂者", "都不可见"])]
    public MediumVisibility ArrowVisibility { get; set; } = MediumVisibility.Both;

    [ModdedEnumOption("被揭示者", typeof(MediateRevealedTargets), ["最早死亡者", "最新死亡者", "随机死亡者", "全部死亡者"])]
    public MediateRevealedTargets WhoIsRevealed { get; set; } = MediateRevealedTargets.AllDead;
}

public enum MediateRevealedTargets
{
    OldestDead,
    NewestDead,
    RandomDead,
    AllDead,
}

public enum MediumVisibility
{
    ShowMedium,
    ShowMediate,
    Both,
    None,
}
