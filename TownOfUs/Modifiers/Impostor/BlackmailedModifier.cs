using MiraAPI.Modifiers;

namespace TownOfUs.Modifiers.Impostor;

public sealed class BlackmailedModifier(byte blackMailerId) : BaseModifier
{
    public override string ModifierName => "被勒索";
    public override bool HideOnUi => true;

    public byte BlackMailerId { get; } = blackMailerId;
    
    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }
}
