using MiraAPI.Events;
using MiraAPI.Modifiers;
using Reactor.Utilities.Extensions;
using TownOfUs.Events.TouEvents;
using TownOfUs.Utilities;

namespace TownOfUs.Modifiers.Neutral;

public sealed class MercenaryBribedModifier(PlayerControl mercenary) : BaseModifier
{
    public override string ModifierName => "雇佣兵贿赂";
    public override bool HideOnUi => true;
    public PlayerControl Mercenary { get; } = mercenary;

    public bool alerted;

    public override void OnActivate()
    {
        base.OnActivate();

        var touAbilityEvent = new TouAbilityEvent(AbilityType.MercenaryBribe, Mercenary, Player);
        MiraEventManager.InvokeEvent(touAbilityEvent);
    }


    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }

    public override void OnMeetingStart()
    {
        if (!Player.AmOwner) return;
        if (alerted) return;

        var title = $"<color=#{TownOfUsColors.Mercenary.ToHtmlStringRGBA()}>雇佣兵反馈</color>";
        MiscUtils.AddFakeChat(Player.Data, title, "你被雇佣兵贿赂了！", false, true);

        alerted = true;
    }
}
