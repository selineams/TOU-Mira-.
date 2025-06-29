using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Utilities;
using UnityEngine;
using static ShipStatus;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class TorchModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "火炬";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Torch;
    public override string GetDescription() => "当灯被破坏时，你的视野不会减少。";
    public override ModifierFaction FactionType => ModifierFaction.CrewmateVisibility;

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.TorchChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.TorchAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate() && Instance.Type != MapType.Fungle;
    }
    public string GetAdvancedDescription()
    {
        return "灯被破坏时不会影响你的视野。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
