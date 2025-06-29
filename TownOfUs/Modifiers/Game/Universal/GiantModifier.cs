using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Universal;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Universal;

public sealed class GiantModifier : UniversalGameModifier, IWikiDiscoverable, IVisualAppearance
{
    public override string ModifierName => "巨人";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Giant;
    public override string GetDescription() => $"你比普通玩家更大，移动速度比普通玩家慢{Math.Round(1f / OptionGroupSingleton<GiantOptions>.Instance.GiantSpeed, 2)}倍。";
    public override ModifierFaction FactionType => ModifierFaction.UniversalVisibility;

    public override int GetAssignmentChance() =>
        (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.GiantChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.GiantAmount;

    public VisualAppearance GetVisualAppearance()
    {
        var appearance = Player.GetDefaultAppearance();
        appearance.Speed = OptionGroupSingleton<GiantOptions>.Instance.GiantSpeed;
        appearance.Size = new Vector3(1f, 1f, 1f);
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
        return $"你比普通玩家更大，移动速度也比普通玩家慢{Math.Round(OptionGroupSingleton<GiantOptions>.Instance.GiantSpeed, 2)}倍。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
