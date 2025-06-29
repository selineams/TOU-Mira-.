using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Universal;

public sealed class TiebreakerModifier : UniversalGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "破平者";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Tiebreaker;
    public override string GetDescription() => "你的投票可以打破平票";
    public override ModifierFaction FactionType => ModifierFaction.UniversalPassive;

    public override int GetAmountPerGame() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.TiebreakerAmount != 0 ? 1 : 0;
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.TiebreakerChance;
    public string GetAdvancedDescription()
    {
        return "你的投票可以打破平票。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
