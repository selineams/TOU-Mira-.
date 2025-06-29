using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Universal;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Universal;

public sealed class MiniModifier : UniversalGameModifier, IWikiDiscoverable, IVisualAppearance
{
    public override string ModifierName => "迷你";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Mini;
    public override string GetDescription() => $"你比普通玩家更小，移动速度比普通玩家快{Math.Round(OptionGroupSingleton<MiniOptions>.Instance.MiniSpeed, 2)}倍。";
    public override ModifierFaction FactionType => ModifierFaction.UniversalVisibility;

    public override int GetAssignmentChance() =>
        (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.MiniChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.MiniAmount;

    public VisualAppearance GetVisualAppearance()
    {
        var appearance = Player.GetDefaultAppearance();
        appearance.Speed = OptionGroupSingleton<MiniOptions>.Instance.MiniSpeed;
        appearance.Size = new Vector3(0.49f, 0.49f, 1f);
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
        return $"你比普通玩家更小，移动速度也比普通玩家快{Math.Round(OptionGroupSingleton<MiniOptions>.Instance.MiniSpeed, 2)}倍。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
