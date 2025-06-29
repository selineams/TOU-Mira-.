using MiraAPI.Modifiers.Types;
using MiraAPI.PluginLoading;

namespace TownOfUs.Modifiers;

// This modifier is used to hide player animations and whatnot, very useful for the shield modifiers
[MiraIgnore]
public abstract class ConcealedModifier : TimedModifier
{
    public override string ModifierName => "隐匿修饰";
    public override string GetDescription() => "你已隐匿！";
    public override float Duration => 1f;
    public override bool AutoStart => false;
    public override bool HideOnUi => true;
    public override void OnDeath(DeathReason reason)
    {
        base.OnDeath(reason);

        ModifierComponent!.RemoveModifier(this);
    }
}
