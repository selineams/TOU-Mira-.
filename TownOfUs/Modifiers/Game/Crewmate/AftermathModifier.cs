using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class AftermathModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "余波";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Aftermath;
    public override string GetDescription() => "你的击杀者会被强制使用技能！";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePostmortem;
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.AftermathChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.AftermathAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate();
    }
    public string GetAdvancedDescription()
    {
        return "你死后，击杀你的玩家会被强制使用技能，目标为你的尸体或他们自己。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
