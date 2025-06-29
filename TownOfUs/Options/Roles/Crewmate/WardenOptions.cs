using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class WardenOptions : AbstractOptionGroup<WardenRole>
{
    public override string GroupName => "护卫者";

    [ModdedEnumOption("强化结界显示对象", typeof(FortifyOptions), ["自身", "护卫者", "自身+护卫者", "所有人"])]
    public FortifyOptions ShowFortified { get; set; } = FortifyOptions.SelfAndWarden;
}

public enum FortifyOptions
{
    Self,
    Warden,
    SelfAndWarden,
    Everyone,
}
