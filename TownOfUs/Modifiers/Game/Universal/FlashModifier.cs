using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Universal;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Universal;

public sealed class FlashModifier : UniversalGameModifier, IWikiDiscoverable, IVisualAppearance
{
    public override string ModifierName => "闪电侠";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Flash;
    public override string GetDescription() => $"你的移动速度是普通玩家的{Math.Round(OptionGroupSingleton<FlashOptions>.Instance.FlashSpeed, 2)}倍。";
    public override ModifierFaction FactionType => ModifierFaction.UniversalVisibility;

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.FlashChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.FlashAmount;
    
    public VisualAppearance GetVisualAppearance()
    {
        var appearance = Player.GetDefaultAppearance();
        appearance.Speed = OptionGroupSingleton<FlashOptions>.Instance.FlashSpeed;
        return appearance;
    }
    public override void OnActivate()
    {
        Player.RawSetAppearance(this);
    }

    public override void OnDeactivate()
    {
        Player?.ResetAppearance(fullReset: true);
    }

    public string GetAdvancedDescription()
    {
        return $"你的移动速度比普通玩家快{Math.Round(OptionGroupSingleton<FlashOptions>.Instance.FlashSpeed, 2)}倍。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
