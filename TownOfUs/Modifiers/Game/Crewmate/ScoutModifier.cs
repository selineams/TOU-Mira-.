using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Utilities;
using UnityEngine;
using static ShipStatus;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class ScoutModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "侦察兵";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Scout;
    public override string GetDescription() => "灯亮时视野更大，灯灭时视野极低。";
    public override ModifierFaction FactionType => ModifierFaction.CrewmateVisibility;

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.ScoutChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.ScoutAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate() && Instance.Type != MapType.Fungle;
    }
    public string GetAdvancedDescription()
    {
        return "你能比普通船员看得更远，但灯灭时视野会大幅下降。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
