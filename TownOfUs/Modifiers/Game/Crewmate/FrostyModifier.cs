using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class FrostyModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "雪人";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Frosty;
    public override string GetDescription() => "让你的击杀者短时间内减速。";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePostmortem;

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.FrostyChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.FrostyAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate();
    }
    public string GetAdvancedDescription()
    {
        return "你死后，击杀你的玩家会被减速！" + MiscUtils.AppendOptionsText(GetType());
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
