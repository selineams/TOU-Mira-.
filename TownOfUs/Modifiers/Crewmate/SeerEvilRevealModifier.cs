using MiraAPI.Modifiers;

namespace TownOfUs.Modifiers.Crewmate;

public sealed class SeerEvilRevealModifier : BaseModifier
{
    public override string ModifierName => "邪恶预言揭示";
    public override bool HideOnUi => true;

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }
}
