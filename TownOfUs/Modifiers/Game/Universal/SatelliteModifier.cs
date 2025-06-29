using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using TownOfUs.Buttons.Modifiers;
using TownOfUs.Modules.Anims;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Universal;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Universal;

public sealed class SatelliteModifier : UniversalGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "卫星";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Satellite;
    public override string GetDescription() => "你可以发射信号，探测地图上所有尸体的位置。";
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.SatelliteChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.SatelliteAmount;
    public override ModifierFaction FactionType => ModifierFaction.UniversalUtility;
    private readonly List<PlayerControl> CastedPlayers = [];
    private readonly List<SpriteRenderer> CastedIcons = [];
    public int Priority { get; set; } = 5;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role is not MysticRole;
    }
    public void OnRoundStart()
    {
        CustomButtonSingleton<SatelliteButton>.Instance.Usable = true;
        ClearMapIcons();
    }

    public void NewMapIcon(PlayerControl player)
    {
        if (!CastedPlayers.Contains(player))
        {
            var newIcon = UnityEngine.Object.Instantiate(MapBehaviour.Instance.TrackedHerePoint);
            newIcon.material = AnimStore.SetSpriteColourMatch(player, newIcon.material);

            Vector3 vector = player.transform.position;
            vector /= ShipStatus.Instance.MapScale;
            vector.x *= Mathf.Sign(ShipStatus.Instance.transform.localScale.x);
            vector.z = -1f;

            newIcon.transform.localPosition = vector;

            CastedPlayers.Add(player);
            CastedIcons.Add(newIcon);
        }
    }

    public void ClearMapIcons()
    {
        foreach (var gameObject in CastedIcons.Select(icon => icon.gameObject).Where(gameObject => gameObject != null))
        {
            gameObject.Destroy();
        }
        CastedIcons.Clear();
    }
    public string GetAdvancedDescription()
    {
        return "你可以发射信号，得知所有尸体的位置。" + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("信号广播",
            $"你可以在地图上检测尸体，每局可用{OptionGroupSingleton<SatelliteOptions>.Instance.MaxNumCast}次。",
            TouAssets.BroadcastSprite),
    ];
}
