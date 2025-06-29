using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using TownOfUs.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Roles.Neutral;
using UnityEngine;
using MiraAPI.Utilities;

namespace TownOfUs.Buttons.Neutral;

public sealed class MercenaryGuardButton : TownOfUsRoleButton<MercenaryRole, PlayerControl>
{
    public override string Name => "保护";
    public override string Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfUsColors.Mercenary;
    public override float Cooldown => OptionGroupSingleton<MercenaryOptions>.Instance.GuardCooldown + MapCooldown;
    public override int MaxUses => (int)OptionGroupSingleton<MercenaryOptions>.Instance.MaxUses;
    public override LoadableAsset<Sprite> Sprite => TouNeutAssets.GuardSprite;

    public override bool Enabled(RoleBehaviour? role) => base.Enabled(role) && !PlayerControl.LocalPlayer.Data.IsDead;
    
    protected override void OnClick()
    {
        if (Target == null)
        {
            Logger<TownOfUsPlugin>.Error("Mercenary Guard: Target is null");
            return;
        }

        Target.RpcAddModifier<MercenaryGuardModifier>(PlayerControl.LocalPlayer);
        var notif1 = Helpers.CreateAndShowNotification($"<b>当{Target.Data.PlayerName}被互动时，你将获得一枚金币。</b>", Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Mercenary.LoadAsset());
        notif1.Text.SetOutlineThickness(0.35f);
    }

    public override PlayerControl? GetTarget() => PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, predicate: x => !x.HasModifier<MercenaryGuardModifier>());
}
