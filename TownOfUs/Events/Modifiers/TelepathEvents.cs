using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfUs.Modifiers.Game.Impostor;
using TownOfUs.Modules;
using TownOfUs.Options.Modifiers.Impostor;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Events.Modifiers;

public static class TelepathEvents
{
    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent @event)
    {

        var source = @event.Source;
        var victim = @event.Target;

        if (PlayerControl.LocalPlayer.HasModifier<TelepathModifier>() && !source.AmOwner && !victim.AmOwner)
        {
            var options = OptionGroupSingleton<TelepathOptions>.Instance;
            if (victim.IsImpostor() && source == victim && options.KnowFailedGuess && MeetingHud.Instance && victim.TryGetModifier<ImpostorAssassinModifier>(out var assassin) && assassin.LastAttemptedVictim)
            {
                Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.ImpSoft, alpha: 0.4f));
                var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.ImpSoft.ToTextColor()}你的队友{victim.Data.PlayerName}尝试以{assassin.LastGuessedItem}身份射击{assassin.LastAttemptedVictim!.Data.PlayerName}，但失败了！</b></color>", Color.white, spr: TouModifierIcons.Telepath.LoadAsset());
                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
            else if (source.IsImpostor() && source != victim && options.KnowCorrectGuess && MeetingHud.Instance)
            {
                Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.ImpSoft, alpha: 0.05f));
                var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.ImpSoft.ToTextColor()}你的队友{source.Data.PlayerName}以{victim.GetRoleWhenAlive().TeamColor.ToTextColor()}{victim.GetRoleWhenAlive().NiceName}</color>身份射杀了{victim.Data.PlayerName}！</b></color>", Color.white, spr: TouModifierIcons.Telepath.LoadAsset());
                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
            else if (source.IsImpostor() && source != victim)
            {
                Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.ImpSoft, alpha: 0.05f));
                var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.ImpSoft.ToTextColor()}你的队友{source.Data.PlayerName}进行了击杀。</b></color>", Color.white, spr: TouModifierIcons.Telepath.LoadAsset());
                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
                if (options.KnowKillLocation) victim?.AddModifier<TelepathDeathNotifierModifier>(PlayerControl.LocalPlayer);
            }
            else if (victim.IsImpostor() && options.KnowDeath)
            {
                Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.ImpSoft, alpha: 0.4f));
                var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.ImpSoft.ToTextColor()}你的队友{victim.Data.PlayerName}被杀害了！</b></color>", Color.white, spr: TouModifierIcons.Telepath.LoadAsset());
                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
                if (options.KnowDeathLocation) victim?.AddModifier<TelepathDeathNotifierModifier>(PlayerControl.LocalPlayer);
            }
        }
    }
}
