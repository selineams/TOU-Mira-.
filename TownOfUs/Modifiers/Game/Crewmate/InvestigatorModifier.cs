using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class InvestigatorModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "调查员";
    public override LoadableAsset<Sprite>? ModifierIcon => TouRoleIcons.Investigator;
    public override string GetDescription() => "你可以看到所有人的脚印。";
    public override ModifierFaction FactionType => ModifierFaction.CrewmateUtility;

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.InvestigatorChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.InvestigatorAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate() && role is not InvestigatorRole;
    }

    public override void OnActivate()
    {
        if (!Player.AmOwner) return;

        Helpers.GetAlivePlayers().Where(plr => !plr.HasModifier<FootstepsModifier>())
            .ToList().ForEach(plr => plr.GetModifierComponent().AddModifier<FootstepsModifier>());
    }

    public override void OnDeactivate()
    {
        if (!Player.AmOwner) return;

        PlayerControl.AllPlayerControls.ToArray().Where(plr => plr.HasModifier<FootstepsModifier>())
            .ToList().ForEach(plr => plr.GetModifierComponent().RemoveModifier<FootstepsModifier>());
    }
    public string GetAdvancedDescription()
    {
        return "调查员可以在游戏中看到所有玩家的脚印，被变形的玩家脚印对调查员不可见。" + MiscUtils.AppendOptionsText(CustomRoleSingleton<InvestigatorRole>.Instance.GetType());
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
