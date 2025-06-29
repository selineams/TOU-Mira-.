using UnityEngine;

namespace TownOfUs.Modifiers.Crewmate;

public sealed class PlumberVenterModifier(PlayerControl owner, Color color) : ArrowTargetModifier(owner, color, 0)
{
    public override string ModifierName => "水管工通风口箭头";
}
