using AmongUs.GameOptions;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Impostor;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Impostor;

public sealed class UnderdogModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "潜伏者";
    public override string GetDescription() => "当你独自一人时，击杀冷却会缩短";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Underdog;
    public override ModifierFaction FactionType => ModifierFaction.ImpostorPassive;

    private static float KillCooldownIncrease => OptionGroupSingleton<UnderdogOptions>.Instance.KillCooldownIncrease;
    private static bool ExtraImpsKillCooldown => OptionGroupSingleton<UnderdogOptions>.Instance.ExtraImpsKillCooldown;

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.UnderdogChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.UnderdogAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsImpostor();
    }

    public static bool LastImpostor()
    {
        return MiscUtils.ImpAliveCount <= 1;
    }

    public static float GetKillCooldown(PlayerControl player)
    {
        var mod = player.GetModifier<UnderdogModifier>();

        if (mod == null)
            return GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);

        var baseKillCooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);

        var lowerKc = baseKillCooldown - KillCooldownIncrease;
        var upperKc = baseKillCooldown + KillCooldownIncrease;

        var kc = ExtraImpsKillCooldown ? upperKc : baseKillCooldown;
        var timer = LastImpostor() ? lowerKc : kc;

        // Logger<TownOfUsPlugin>.Error($"GetKillCooldown({player.Data.PlayerName}) baseKillCooldown: {baseKillCooldown}, baseKillCooldown2: {baseKillCooldown2}, lowerKc {lowerKc}, upperKc {upperKc}, kc {kc}, timer {timer}");

        return timer;
    }
    public string GetAdvancedDescription()
    {
        return "当你单独行动或队友死亡时，击杀冷却会降低。" + MiscUtils.AppendOptionsText(GetType());
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
