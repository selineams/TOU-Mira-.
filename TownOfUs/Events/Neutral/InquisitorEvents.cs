using HarmonyLib;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Events.Neutral;

public static class InquisitorEvents
{
    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent @event)
    {
        var source = @event.Source;
        var victim = @event.Target;

        if (source.Data.Role is InquisitorRole inquis && GameHistory.PlayerStats.TryGetValue(source.PlayerId, out var stats))
        {
            if (victim.HasModifier<InquisitorHereticModifier>())
            {
                stats.CorrectKills += 1;
            }
            else if (source != victim)
            {
                stats.IncorrectKills += 1;
                inquis.CanVanquish = false;
            }
        }

        if (PlayerControl.LocalPlayer.Data.Role is InquisitorRole)
        {
            if (victim.HasModifier<InquisitorHereticModifier>() && !victim.AmOwner && !source.AmOwner)
            {
                Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Inquisitor, alpha: 0.1f));
                var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.Inquisitor.ToTextColor()}异端已被消灭！</b></color>", Color.white, spr: TouRoleIcons.Inquisitor.LoadAsset());
                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
            else if (!victim.HasModifier<InquisitorHereticModifier>() && !victim.AmOwner && source.AmOwner)
            {
                Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Inquisitor, alpha: 0.4f));
                var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.Inquisitor.ToTextColor()}{victim.Data.PlayerName}不是异端！\n你无法再消灭其他玩家。</b></color>", Color.white, spr: TouRoleIcons.Inquisitor.LoadAsset());
                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
            else if (victim.HasModifier<InquisitorHereticModifier>() && !victim.AmOwner && source.AmOwner)
            {
                Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Doomsayer, alpha: 0.4f));
                var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.Inquisitor.ToTextColor()}{victim.Data.PlayerName}是异端！</b></color>", Color.white, spr: TouRoleIcons.Inquisitor.LoadAsset());
                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
        }
        CustomRoleUtils.GetActiveRolesOfType<InquisitorRole>().Do(x => x.CheckTargetDeath(victim));
    }

    [RegisterEvent]
    public static void PlayerDeathEventHandler(PlayerDeathEvent @event)
    {
        if (@event.DeathReason != DeathReason.Exile) return;

        CustomRoleUtils.GetActiveRolesOfType<InquisitorRole>().Do(x => x.CheckTargetDeath(@event.Player));
    }

    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent @event)
    {
        if (@event.TriggeredByIntro) return;
        var inquis = CustomRoleUtils.GetActiveRolesOfType<InquisitorRole>().FirstOrDefault();
        if (inquis != null && inquis.TargetsDead && !inquis.Player.HasDied())
        {
            if (inquis.Player.AmOwner)
            {
                PlayerControl.LocalPlayer.RpcPlayerExile();
                var notif1 = Helpers.CreateAndShowNotification(
                    $"<b>你作为{TownOfUsColors.Inquisitor.ToTextColor()}审判官</color>已成功获胜，所有异端已被消灭！</b>", Color.white, spr: TouRoleIcons.Inquisitor.LoadAsset());

                notif1.Text.SetOutlineThickness(0.35f);
                    notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
            else
            {
                var notif1 = Helpers.CreateAndShowNotification(
                    $"<b>{TownOfUsColors.Inquisitor.ToTextColor()}审判官</color> {inquis.Player.Data.PlayerName} 已成功获胜，所有异端已被消灭！</b>", Color.white, spr: TouRoleIcons.Inquisitor.LoadAsset());

                notif1.Text.SetOutlineThickness(0.35f);
                    notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
        }
    }
}
