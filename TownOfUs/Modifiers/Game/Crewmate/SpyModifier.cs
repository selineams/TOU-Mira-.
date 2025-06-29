using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using TownOfUs.Buttons.Modifiers;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;
using static ShipStatus;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class SpyModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "特工";
    public override LoadableAsset<Sprite>? ModifierIcon => TouRoleIcons.Spy;
    public override string GetDescription() => "在管理台获得额外信息。";
    public override ModifierFaction FactionType => ModifierFaction.CrewmateUtility;
    public override void OnActivate()
    {
        base.OnActivate();

        if (!Player.AmOwner) return;
        CustomButtonSingleton<SpyAdminTableModifierButton>.Instance.AvailableCharge = OptionGroupSingleton<SpyOptions>.Instance.StartingCharge.Value;
    }
    public static void OnRoundStart()
    {
        CustomButtonSingleton<SpyAdminTableModifierButton>.Instance.AvailableCharge += OptionGroupSingleton<SpyOptions>.Instance.RoundCharge.Value;
    }
    public static void OnTaskComplete()
    {
        CustomButtonSingleton<SpyAdminTableModifierButton>.Instance.AvailableCharge += OptionGroupSingleton<SpyOptions>.Instance.TaskCharge.Value;
    }

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.SpyChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.SpyAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role is not SpyRole && role.IsCrewmate() && Instance.Type != MapType.Fungle;
    }
    public string GetAdvancedDescription()
    {
        return "特工在管理台可以获得额外信息，不仅能看到每个房间的人数，还能看到每个房间里都有谁。" + MiscUtils.AppendOptionsText(CustomRoleSingleton<SpyRole>.Instance.GetType());
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
