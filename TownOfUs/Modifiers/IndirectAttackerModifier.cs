using MiraAPI.Modifiers;

namespace TownOfUs.Modifiers;

public sealed class IndirectAttackerModifier(bool ignoreShield) : BaseModifier
{
    public override string ModifierName => "间接攻击者";
    public override bool HideOnUi => true;
    public bool IgnoreShield => ignoreShield;

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }
}
