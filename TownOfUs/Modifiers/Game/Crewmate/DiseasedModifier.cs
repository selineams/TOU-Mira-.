using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class DiseasedModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "病人";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Diseased;
    public override string GetDescription() => "让你的击杀者击杀冷却增加。";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePostmortem;

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.DiseasedChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.DiseasedAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate();
    }
    public string GetAdvancedDescription()
    {
        return
            $"你死后，击杀你的玩家击杀冷却将乘以{OptionGroupSingleton<DiseasedOptions>.Instance.CooldownMultiplier}倍。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
