using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Components;
using TownOfUs.Roles.Crewmate;
using UnityEngine;

namespace TownOfUs.Buttons.Crewmate;

public sealed class DetectiveInspectButton : TownOfUsRoleButton<DetectiveRole, CrimeSceneComponent>
{
    public override string Name => "调查";
    public override string Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfUsColors.Detective;
    public override float Cooldown => 1f + MapCooldown;
    public override LoadableAsset<Sprite> Sprite => TouCrewAssets.InspectSprite;

    public override CrimeSceneComponent? GetTarget() => PlayerControl.LocalPlayer.GetNearestObjectOfType<CrimeSceneComponent>(Distance, Helpers.CreateFilter(Constants.NotShipMask));

    public override void SetOutline(bool active)
    {
        // placeholder
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }

        Role.InvestigatingScene = Target;
        Role.InvestigatedPlayers.AddRange(Target.GetScenePlayers());
        var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.Detective.ToTextColor()}你已调查了{Target.DeadPlayer!.Data.PlayerName}的案发现场。凶手或任何进入案发现场的人在被检查时会闪红光。</b></color>", Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Detective.LoadAsset());
        notif1.Text.SetOutlineThickness(0.35f);
        // TouAudio.PlaySound(TouAudio.QuestionSound);
    }
}
