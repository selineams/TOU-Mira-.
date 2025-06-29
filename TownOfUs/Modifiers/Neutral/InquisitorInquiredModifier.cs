using MiraAPI.Modifiers;

namespace TownOfUs.Modifiers.Neutral;

public sealed class InquisitorInquiredModifier : BaseModifier
{
    public override string ModifierName => "审问";
    public override bool HideOnUi => true;
}
