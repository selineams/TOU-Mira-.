using MiraAPI.Modifiers;
using MiraAPI.PluginLoading;

namespace TownOfUs.Modifiers;

[MiraIgnore]
public abstract class PlayerTargetModifier(byte ownerId) : BaseModifier
{
    public override string ModifierName => "目标";
    public override bool HideOnUi => true;

    public byte OwnerId { get; } = ownerId;
}
