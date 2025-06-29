using System.Collections;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class BaitModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "诱饵";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Bait;
    public override string GetDescription() => "让你的击杀者强制自报尸体。";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePostmortem;

    private static float MinDelay => OptionGroupSingleton<BaitOptions>.Instance.MinDelay;
    private static float MaxDelay => OptionGroupSingleton<BaitOptions>.Instance.MaxDelay;

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.BaitChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.BaitAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate();
    }

    public static IEnumerator CoReportDelay(PlayerControl killer, PlayerControl target)
    {
        if (!killer || !target || killer == target)
            yield break;

        yield return new WaitForSeconds(Random.RandomRange(MinDelay, MaxDelay));

        if (killer.AmOwner)
        {
            killer.CmdReportDeadBody(target.Data);

            var notif1 = Helpers.CreateAndShowNotification(
                $"<b>{TownOfUsColors.Bait.ToTextColor()}{target.Data.PlayerName}是诱饵，导致你强制自报尸体。</color></b>", Color.white, spr: TouModifierIcons.Bait.LoadAsset());

            notif1.Text.SetOutlineThickness(0.35f);
            notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
        }
    }
    public string GetAdvancedDescription()
    {
        return "你死后，击杀你的玩家会强制自报你的尸体。" + MiscUtils.AppendOptionsText(GetType());
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
