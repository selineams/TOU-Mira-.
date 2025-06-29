using MiraAPI.Modifiers;

namespace TownOfUs.Modifiers.Crewmate;

public sealed class JailSparedModifier(byte jailorId) : BaseModifier
{
    public override string ModifierName => "免囚";
    public override bool HideOnUi => true;
    public byte JailorId { get; } = jailorId;
    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }
}
