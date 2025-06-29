using MiraAPI.Modifiers;

namespace TownOfUs.Modifiers.Impostor;

public sealed class BlackmailSparedModifier(byte blackMailerId) : BaseModifier
{
    public override string ModifierName => "被勒索";
    public override bool HideOnUi => true;

    public byte BlackMailerId { get; } = blackMailerId;
}
