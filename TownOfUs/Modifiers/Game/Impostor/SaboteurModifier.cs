using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Impostor;

public sealed class SaboteurModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "破坏者";
    public override string GetDescription() => "你的破坏技能冷却减少";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Saboteur;
    public override ModifierFaction FactionType => ModifierFaction.ImpostorPassive;

    public float Timer { get; set; }

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.SaboteurChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.SaboteurAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsImpostor();
    }
    public string GetAdvancedDescription()
    {
        return "你破坏时冷却减少。" + MiscUtils.AppendOptionsText(GetType());
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
