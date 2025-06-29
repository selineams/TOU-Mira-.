using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class ProsecutorOptions : AbstractOptionGroup<ProsecutorRole>
{
    public override string GroupName => "检察官";

    [ModdedToggleOption("检察官放逐船员时死亡")]
    public bool ExileOnCrewmate { get; set; } = true;

    [ModdedNumberOption("最大检举次数", 1, 5)]
    public float MaxProsecutions { get; set; } = 1f;
}
