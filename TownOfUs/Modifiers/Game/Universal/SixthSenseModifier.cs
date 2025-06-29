using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Roles.Crewmate;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Universal;

public sealed class SixthSenseModifier : UniversalGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "第六感";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.SixthSense;
    public override string GetDescription() => "有人对你使用技能时你会知晓。";
    public override ModifierFaction FactionType => ModifierFaction.UniversalPassive;

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.SixthSenseChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.SixthSenseAmount;
    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role is not AurialRole;
    }
    public string GetAdvancedDescription()
    {
        return "有人对你使用技能时你会知晓。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
