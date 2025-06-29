using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modifiers.Game.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Events.Modifiers;

public static class EgotistEvents
{
    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent @event)
    {
        if (@event.TriggeredByIntro) return;
        
        var ego = ModifierUtils.GetActiveModifiers<EgotistModifier>().FirstOrDefault(x => !x.Player.HasDied());
        if (ego != null && Helpers.GetAlivePlayers().Where(x => x.IsCrewmate() && !x.HasModifier<AllianceGameModifier>()).ToList().Count == 0)
        {
            if (ego.Player.AmOwner)
            {
                PlayerControl.LocalPlayer.RpcPlayerExile();
                var notif1 = Helpers.CreateAndShowNotification(
                    $"<b>你以{TownOfUsColors.Egotist.ToTextColor()}营己徒</color>的身份成功获胜，因为没有船员存活！</b>", Color.white, spr: TouModifierIcons.Egotist.LoadAsset());

                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
            else
            {
                var notif1 = Helpers.CreateAndShowNotification(
                    $"<b>{TownOfUsColors.Egotist.ToTextColor()}营己徒</color>，{ego.Player.Data.PlayerName}，因没有船员存活而成功获胜！</b>", Color.white, spr: TouModifierIcons.Egotist.LoadAsset());

                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
        }
    }
}
