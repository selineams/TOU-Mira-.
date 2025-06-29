using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Events.Neutral;

public static class DoomsayerEvents
{
    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent @event)
    {
        if (@event.Source.Data.Role is DoomsayerRole doom && @event.Source.AmOwner && (int)OptionGroupSingleton<DoomsayerOptions>.Instance.DoomsayerGuessesToWin == doom.NumberOfGuesses)
        {
            DoomsayerRole.RpcDoomsayerWin(@event.Source);
        }
    }

    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent @event)
    {
        if (@event.TriggeredByIntro) return;
        if (OptionGroupSingleton<DoomsayerOptions>.Instance.DoomWin is not DoomWinOptions.Leaves) return;

        var doom = CustomRoleUtils.GetActiveRolesOfType<DoomsayerRole>().FirstOrDefault(x => x.AllGuessesCorrect && !x.Player.HasDied());
        if (doom != null)
        {
            if (doom.Player.AmOwner)
            {
                PlayerControl.LocalPlayer.RpcPlayerExile();
                var notif1 = Helpers.CreateAndShowNotification(
                    $"<b>你以{TownOfUsColors.Doomsayer.ToTextColor()}末日预言家</color>的身份成功获胜，因为你已成功猜中足够的玩家！</b>", Color.white, spr: TouRoleIcons.Doomsayer.LoadAsset());

                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
            else
            {
                var notif1 = Helpers.CreateAndShowNotification(
                    $"<b>{TownOfUsColors.Doomsayer.ToTextColor()}末日预言家</color>，{doom.Player.Data.PlayerName}，因成功猜中足够的玩家而获胜！</b>", Color.white, spr: TouRoleIcons.Doomsayer.LoadAsset());

                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
        }
    }
}
