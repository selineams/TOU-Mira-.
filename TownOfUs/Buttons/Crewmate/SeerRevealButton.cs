using System.Text;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Buttons.Crewmate;

public sealed class SeerRevealButton : TownOfUsRoleButton<SeerRole, PlayerControl>
{
    public override string Name => "揭示";
    public override string Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfUsColors.Seer;
    public override float Cooldown => OptionGroupSingleton<SeerOptions>.Instance.SeerCooldown + MapCooldown;
    public override LoadableAsset<Sprite> Sprite => TouCrewAssets.SeerSprite;

    public override bool IsTargetValid(PlayerControl? target)
    {
        return base.IsTargetValid(target) && !target!.HasModifier<SeerGoodRevealModifier>() && !target!.HasModifier<SeerEvilRevealModifier>();
    }

    public override PlayerControl? GetTarget() => PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);

    protected override void OnClick()
    {
        if (Target == null) return;

        RevealAlliance(Target);
        TouAudio.PlaySound(TouAudio.QuestionSound);

        Target?.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>(TownOfUsColors.Seer));
    }

    public static void RevealAlliance(PlayerControl target)
    {
        var options = OptionGroupSingleton<SeerOptions>.Instance;
        var possibleAlignment = new StringBuilder();

        if (IsEvil(target))
        {
            target.AddModifier<SeerEvilRevealModifier>();
            var possiblyGood = options.ShowCrewmateKillingAsRed ? "可能" : string.Empty;
            if (options.ShowNeutralBenignAsRed) possiblyGood = "可能";

            var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.ImpSoft.ToTextColor()}你揭示了{target.Data.PlayerName}可能是邪恶阵营！</color></b>", Color.white, spr: TouRoleIcons.Seer.LoadAsset());
            notif1.Text.SetOutlineThickness(0.35f);
            notif1.transform.localPosition = new Vector3(0f, 1f, -20f);

            if (options.ShowCrewmateKillingAsRed) possibleAlignment.Append("船员击杀者, ");
            if (options.ShowNeutralBenignAsRed) possibleAlignment.Append("中立善良, ");
            if (options.ShowNeutralEvilAsRed) possibleAlignment.Append("中立邪恶, ");
            if (options.ShowNeutralKillingAsRed) possibleAlignment.Append("中立击杀者, ");
            if (options.SwapTraitorColors) possibleAlignment.Append("叛徒, ");

            if (possibleAlignment.Length > 3)
                possibleAlignment = possibleAlignment.Remove(possibleAlignment.Length - 2, 2);
            var impString = possibleAlignment.Length > 1 ? "，或内鬼！" : "内鬼！";
            possibleAlignment.Append(impString);

            Helpers.CreateAndShowNotification($"他们一定是{possibleAlignment}", TownOfUsColors.ImpSoft);
        }
        else
        {
            target.AddModifier<SeerGoodRevealModifier>();
            var possiblyGood = !options.ShowNeutralBenignAsRed ? "很可能" : string.Empty;
            if (!options.ShowNeutralEvilAsRed) possiblyGood = "大概率";
            if (!options.ShowNeutralKillingAsRed) possiblyGood = "可能";

            var notif1 = Helpers.CreateAndShowNotification($"<b>{Palette.CrewmateBlue.ToTextColor()}你揭示了{target.Data.PlayerName}{possiblyGood}是好人！</color></b>", Color.white, spr: TouRoleIcons.Seer.LoadAsset());
            notif1.Text.SetOutlineThickness(0.35f);
            notif1.transform.localPosition = new Vector3(0f, 1f, -20f);

            if (!options.ShowNeutralBenignAsRed) possibleAlignment.Append("中立善良, ");
            if (!options.ShowNeutralEvilAsRed) possibleAlignment.Append("中立邪恶, ");
            if (!options.ShowNeutralKillingAsRed) possibleAlignment.Append("中立击杀者, ");

            if (possibleAlignment.Length > 3)
                possibleAlignment = possibleAlignment.Remove(possibleAlignment.Length - 2, 2);
            var impString = possibleAlignment.Length > 1 ? "，或船员！" : "船员！";
            possibleAlignment.Append(impString);
            var notif2 = Helpers.CreateAndShowNotification($"<b>他们一定是{possibleAlignment}</b>", Palette.CrewmateBlue);
            notif2.Text.SetOutlineThickness(0.35f);
            notif2.transform.localPosition = new Vector3(0f, 1f, -20f);
        }
    }

    public static bool IsEvil(PlayerControl target)
    {
        var options = OptionGroupSingleton<SeerOptions>.Instance;
        return !target.HasModifier<ImitatorCacheModifier>() && 
            ((target.Is(RoleAlignment.CrewmateKilling) && options.ShowCrewmateKillingAsRed) ||
            (target.Is(RoleAlignment.NeutralBenign) && options.ShowNeutralBenignAsRed) ||
            (target.Is(RoleAlignment.NeutralEvil) && options.ShowNeutralEvilAsRed) ||
            (target.Is(RoleAlignment.NeutralKilling) && options.ShowNeutralKillingAsRed) ||
            (target.IsImpostor() && !target.HasModifier<TraitorCacheModifier>()) ||
            (target.HasModifier<TraitorCacheModifier>() && options.SwapTraitorColors));
    }
}
