using MiraAPI.Modifiers;

namespace TownOfUs.Modifiers.Neutral;

public sealed class VampireBittenModifier : BaseModifier
{
    public override string ModifierName => "被咬";
    public override bool HideOnUi => true;
}
