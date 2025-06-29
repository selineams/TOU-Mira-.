using System.Collections;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfUs.Modules.Components;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class RottingModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "腐烂";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Rotting;
    public override string GetDescription() => $"你的尸体将在{OptionGroupSingleton<RottingOptions>.Instance.RotDelay}秒后腐烂消失。";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePostmortem;

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.RottingChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.RottingAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate();
    }
    public static IEnumerator StartRotting(PlayerControl player)
    {
        yield return new WaitForSeconds(OptionGroupSingleton<RottingOptions>.Instance.RotDelay);
        var rotting = GameObject.FindObjectsOfType<DeadBody>().FirstOrDefault(x => x.ParentId == player.PlayerId);
        if (rotting == null) yield break;
        Coroutines.Start(rotting.CoClean());
        Coroutines.Start(CrimeSceneComponent.CoClean(rotting));
    }
    public string GetAdvancedDescription()
    {
        return $"{OptionGroupSingleton<RottingOptions>.Instance.RotDelay}秒后，你的尸体会腐烂，无法被报告。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
