using MiraAPI.Events;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfUs.Events.TouEvents;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Crewmate;

public sealed class DeputyCampedModifier(PlayerControl deputy) : BaseModifier
{
    public override string ModifierName => "被蹲守";
    public override bool HideOnUi => true;

    public PlayerControl Deputy { get; } = deputy;

    public override void OnActivate()
    {
        base.OnActivate();

        var touAbilityEvent = new TouAbilityEvent(AbilityType.DeputyCamp, Deputy, Player);
        MiraEventManager.InvokeEvent(touAbilityEvent);
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Deputy.AmOwner)
        {
            Player?.cosmetics.SetOutline(true, new Il2CppSystem.Nullable<Color>(TownOfUsColors.Deputy));
        }
    }

    public override void OnDeath(DeathReason reason)
    {
        Player.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>(TownOfUsColors.Deputy));
        if (Deputy.AmOwner)
        {
            var notif1 = Helpers.CreateAndShowNotification(
                $"<b>{TownOfUsColors.Deputy.ToTextColor()}你蹲守的目标 {Player.Data.PlayerName} 已死亡！请在会议中为其复仇。</color></b>",
                Color.white, spr: TouRoleIcons.Deputy.LoadAsset());

            notif1.Text.SetOutlineThickness(0.35f);
            notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Deputy));
        }
    }

    public override void OnDeactivate()
    {
        Player.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>(TownOfUsColors.Deputy));
    }
}
