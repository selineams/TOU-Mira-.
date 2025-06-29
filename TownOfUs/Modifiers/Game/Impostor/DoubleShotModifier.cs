using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Impostor;

public sealed class DoubleShotModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "专业刺客";
    public override string GetDescription() => "刺杀失败时可再获得一次机会";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.DoubleShot;
    public override ModifierFaction FactionType => ModifierFaction.KillerUtility;

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.DoubleShotChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.DoubleShotAmount;

    public bool Used { get; set; }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        if (
            role.Player.IsImpostor() 
            && role.Player.GetModifierComponent().HasModifier<ImpostorAssassinModifier>(true)
            && base.IsModifierValidOn(role)
            )
        {
            return true;
        }
        return false;
    }
    public string GetAdvancedDescription()
    {
        return "刺杀失败时可再获得一次机会。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
