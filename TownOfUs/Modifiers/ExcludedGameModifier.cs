using MiraAPI.Modifiers.Types;
using MiraAPI.PluginLoading;

namespace TownOfUs.Modifiers;

[MiraIgnore]
public abstract class ExcludedGameModifier : GameModifier
{
    public override string ModifierName => "不在灵魂菜单中";
    public override bool HideOnUi => true;
    public override int GetAmountPerGame() => 0;
    public override int GetAssignmentChance() => 0;
}
