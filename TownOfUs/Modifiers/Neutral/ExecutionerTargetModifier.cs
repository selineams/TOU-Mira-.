namespace TownOfUs.Modifiers.Neutral;

public sealed class ExecutionerTargetModifier(byte exeId) : PlayerTargetModifier(exeId)
{
    public override string ModifierName => "行刑目标";
}
