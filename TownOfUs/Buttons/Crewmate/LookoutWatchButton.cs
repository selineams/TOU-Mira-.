using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using TownOfUs.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Crewmate;
using UnityEngine;
using MiraAPI.Utilities;

namespace TownOfUs.Buttons.Crewmate;

public sealed class WatchButton : TownOfUsRoleButton<LookoutRole, PlayerControl>
{
    public override string Name => "观测";
    public override string Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfUsColors.Lookout;
    public override float Cooldown => OptionGroupSingleton<LookoutOptions>.Instance.WatchCooldown + MapCooldown;
    public override int MaxUses => (int)OptionGroupSingleton<LookoutOptions>.Instance.MaxWatches;
    public override LoadableAsset<Sprite> Sprite => TouCrewAssets.WatchSprite;
    public int ExtraUses { get; set; }

    public override bool IsTargetValid(PlayerControl? target)
    {
        return base.IsTargetValid(target) && !target!.HasModifier<LookoutWatchedModifier>(x => x.Lookout == PlayerControl.LocalPlayer);
    }

    public override PlayerControl? GetTarget() => PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);

    protected override void OnClick()
    {
        if (Target == null)
        {
            Logger<TownOfUsPlugin>.Error("Watch: Target is null");
            return;
        }

        Target.RpcAddModifier<LookoutWatchedModifier>(PlayerControl.LocalPlayer);  
        
        var notif1 = Helpers.CreateAndShowNotification($"<b>如果{Target.Data.PlayerName}在下次会议前未死亡，你将得知与其互动的所有职业。</b>", Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Lookout.LoadAsset());
        notif1.Text.SetOutlineThickness(0.35f);
    }
}
