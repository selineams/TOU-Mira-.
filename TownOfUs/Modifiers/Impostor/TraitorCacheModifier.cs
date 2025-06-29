using AmongUs.GameOptions;
using MiraAPI.Events;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs.Events.TouEvents;
using TownOfUs.Roles.Impostor;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Impostor;

public sealed class TraitorCacheModifier : BaseModifier, ICachedRole
{
    public override string ModifierName => "背叛";
    public override bool HideOnUi => true;
    public bool ShowCurrentRoleFirst => true;
    public RoleBehaviour CachedRole => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<TraitorRole>());

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent?.RemoveModifier(this);
    }
    public override void OnActivate()
    {
        if (Player.AmOwner)
        {
            var notif1 = Helpers.CreateAndShowNotification(
                $"<b>{TownOfUsColors.ImpSoft.ToTextColor()}你获得了新身份，现在只能被猜测为背叛者！</color></b>", Color.white, spr: TouRoleIcons.Traitor.LoadAsset());

            notif1.Text.SetOutlineThickness(0.35f);
            notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
        }
        var touAbilityEvent = new TouAbilityEvent(AbilityType.TraitorChangeRole, Player);
        MiraEventManager.InvokeEvent(touAbilityEvent);
    }

    public override void OnDeactivate()
    {
        if (Player.IsRole<TraitorRole>()) return;

        Player.RpcChangeRole(RoleId.Get<TraitorRole>(), false);
    }
}
