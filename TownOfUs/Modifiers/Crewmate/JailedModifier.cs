using MiraAPI.Events;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities.Extensions;
using TownOfUs.Events.TouEvents;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Crewmate;

public sealed class JailedModifier(byte jailorId) : BaseModifier
{
    public override string ModifierName => "被囚禁";
    public override bool HideOnUi => true;
    public byte JailorId { get; } = jailorId;
    private GameObject? jailCell;
    public override void OnActivate()
    {
        base.OnActivate();
        var jailor = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.PlayerId == JailorId);
        var touAbilityEvent = new TouAbilityEvent(AbilityType.JailorJail, jailor!, Player);
        MiraEventManager.InvokeEvent(touAbilityEvent);
    }
    public override void OnMeetingStart()
    {
        Clear();
        if (GameData.Instance.GetPlayerById(JailorId).Object.HasDied() || Player.HasDied() || !MeetingHud.Instance) return;

        if (Player.AmOwner)
        {
            var title = $"<color=#{TownOfUsColors.Jailor.ToHtmlStringRGBA()}>囚禁反馈</color>";
            var text = "你被囚禁了，请在私聊框中说服典狱长你是船员，以避免被处决。";
            if (PlayerControl.LocalPlayer.Is(ModdedRoleTeams.Crewmate)) text = "你被囚禁了，请在私聊框中向典狱长提供有力信息证明你是船员。";
            MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, title, text, false, true);

            var notif1 = Helpers.CreateAndShowNotification(
                $"<b>{TownOfUsColors.Jailor.ToTextColor()}{text}</color></b>", Color.white, spr: TouRoleIcons.Jailor.LoadAsset());

            notif1.Text.SetOutlineThickness(0.35f);
            notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
        }

        foreach (var voteArea in MeetingHud.Instance.playerStates)
        {
            if (Player.PlayerId == voteArea.TargetPlayerId)
            {
                GenCell(voteArea);
            }
        }
    }
    public void Clear()
    {
        jailCell?.Destroy();
    }
    private void GenCell(PlayerVoteArea voteArea)
    {
        var confirmButton = voteArea.Buttons.transform.GetChild(0).gameObject;
        var parent = confirmButton.transform.parent.parent;

        var jailCellObj = UnityEngine.Object.Instantiate(confirmButton, voteArea.transform);

        var cellRenderer = jailCellObj.GetComponent<SpriteRenderer>();
        cellRenderer.sprite = TouAssets.InJailSprite.LoadAsset();

        jailCellObj.transform.localPosition = new Vector3(-0.95f, 0f, -2f);
        jailCellObj.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        jailCellObj.layer = 5;
        jailCellObj.transform.parent = parent;
        jailCellObj.transform.GetChild(0).gameObject.Destroy();

        var passive = jailCellObj.GetComponent<PassiveButton>();
        passive.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();

        jailCell = jailCellObj;
    }
    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }
}
