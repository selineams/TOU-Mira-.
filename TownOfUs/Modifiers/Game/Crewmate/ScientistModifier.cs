using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfUs.Buttons.Modifiers;
using TownOfUs.Modifiers.Game.Universal;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class ScientistModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "科学家";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Scientist;
    public override string GetDescription() => $"可随时随地查看生命体征，只要有电量";
    public override ModifierFaction FactionType => ModifierFaction.CrewmateUtility;
    public override void OnActivate()
    {
        base.OnActivate();

        if (!Player.AmOwner) return;
        CustomButtonSingleton<ScientistButton>.Instance.AvailableCharge = OptionGroupSingleton<ScientistOptions>.Instance.StartingCharge;
    }
    public static void OnRoundStart()
    {
        CustomButtonSingleton<ScientistButton>.Instance.AvailableCharge += OptionGroupSingleton<ScientistOptions>.Instance.RoundCharge;
    }
    public static void OnTaskComplete()
    {
        CustomButtonSingleton<ScientistButton>.Instance.AvailableCharge += OptionGroupSingleton<ScientistOptions>.Instance.TaskCharge;
    }

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.ScientistChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.ScientistAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
	{
		return base.IsModifierValidOn(role) && role.IsCrewmate() && role is not ScientistRole && !role.Player.GetModifierComponent().HasModifier<SatelliteModifier>(true) && !role.Player.GetModifierComponent().HasModifier<ButtonBarryModifier>(true);
	}
    public string GetAdvancedDescription()
    {
        return $"可随时查看生命体征，但有电量限制。" + MiscUtils.AppendOptionsText(GetType());
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
