namespace TownOfUs.Modifiers.Neutral;

public sealed class GuardianAngelTargetModifier(byte gaId) : PlayerTargetModifier(gaId)
{
    public override string ModifierName => "守护天使目标";
}
