using MiraAPI.Modifiers;

namespace TownOfUs.Modifiers;

public sealed class VentModifier : BaseModifier
{
    public override string ModifierName => "钻洞";
    public override bool HideOnUi => true;
    public override bool? CanVent() => true;
}
