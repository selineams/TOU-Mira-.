using MiraAPI.Events;
using MiraAPI.Modifiers;
using Reactor.Utilities.Extensions;
using System.Text;
using TownOfUs.Events.TouEvents;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Crewmate;

public sealed class LookoutWatchedModifier(PlayerControl lookout) : BaseModifier
{
    public override string ModifierName => "被观测";
    public override bool HideOnUi => true;

    public PlayerControl Lookout { get; set; } = lookout;
    public List<RoleBehaviour> SeenPlayers { get; set; } = [];

    public override void OnActivate()
    {
        base.OnActivate();

        var touAbilityEvent = new TouAbilityEvent(AbilityType.LookoutWatch, Lookout, Player);
        MiraEventManager.InvokeEvent(touAbilityEvent);
    }
    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Lookout.AmOwner)
        {
            Player?.cosmetics.SetOutline(true, new Il2CppSystem.Nullable<Color>(TownOfUsColors.Lookout));
        }
    }

    public override void OnMeetingStart()
    {
        if (!Lookout.AmOwner) return;
        var title = $"<color=#{TownOfUsColors.Lookout.ToHtmlStringRGBA()}>观测者反馈</color>";
        var msg = $"没有玩家与{Player.Data.PlayerName}互动";

        if (SeenPlayers.Count != 0)
        {
            var message = new StringBuilder($"与{Player.Data.PlayerName}互动的角色:\n");

            SeenPlayers.Shuffle();

            foreach (var role in SeenPlayers)
            {
                message.Append(TownOfUsPlugin.Culture, $"{role.NiceName}, ");
            }

            message = message.Remove(message.Length - 2, 2);

            var final = message.ToString();

            if (string.IsNullOrWhiteSpace(final))
                return;

            msg = final;
        }

        MiscUtils.AddFakeChat(Player.Data, title, msg, false, true);

        SeenPlayers.Clear();
    }
}
