using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class SpyOptions : AbstractOptionGroup<SpyRole>
{
    public override string GroupName => "特工";

    [ModdedEnumOption("谁可在管理台看到尸体", typeof(AdminDeadPlayers), ["无", "特工", "除特工外所有人", "所有人"])]
    public AdminDeadPlayers WhoSeesDead { get; set; } = AdminDeadPlayers.Spy;

    [ModdedEnumOption("便携管理台可用对象", typeof(PortableAdmin), ["职业", "修饰符", "职业和修饰符", "禁用"])]
    public PortableAdmin HasPortableAdmin { get; set; } = PortableAdmin.Both;

    public ModdedToggleOption MoveWithMenu { get; } = new ModdedToggleOption("使用便携管理台时可移动", true)
    {
        Visible = () => OptionGroupSingleton<SpyOptions>.Instance.HasPortableAdmin is not PortableAdmin.None,
    };
    public ModdedNumberOption StartingCharge { get; } = new ModdedNumberOption("初始电量", 30f, 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)
    {
        Visible = () => OptionGroupSingleton<SpyOptions>.Instance.HasPortableAdmin is not PortableAdmin.None,
    };

    public ModdedNumberOption RoundCharge { get; } = new ModdedNumberOption("每回合充电量", 15f, 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)
    {
        Visible = () => OptionGroupSingleton<SpyOptions>.Instance.HasPortableAdmin is not PortableAdmin.None,
    };

    public ModdedNumberOption TaskCharge { get; } = new ModdedNumberOption("每任务充电量", 15f, 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)
    {
        Visible = () => OptionGroupSingleton<SpyOptions>.Instance.HasPortableAdmin is not PortableAdmin.None,
    };

    public ModdedNumberOption DisplayCooldown { get; } = new ModdedNumberOption("便携管理台显示冷却", 5f, 0f, 30f, 5f, MiraNumberSuffixes.Seconds)
    {
        Visible = () => OptionGroupSingleton<SpyOptions>.Instance.HasPortableAdmin is not PortableAdmin.None,
    };

    public ModdedNumberOption DisplayDuration { get; } = new ModdedNumberOption("便携管理台显示时长", 30f, 0f, 30f, 5f, MiraNumberSuffixes.Seconds, zeroInfinity: true)
    {
        Visible = () => OptionGroupSingleton<SpyOptions>.Instance.HasPortableAdmin is not PortableAdmin.None,
    };
}

public enum PortableAdmin
{
    Role,
    Modifier,
    Both,
    None,
}


public enum AdminDeadPlayers
{
    Nobody,
    Spy,
    EveryoneButSpy,
    Everyone,
}
