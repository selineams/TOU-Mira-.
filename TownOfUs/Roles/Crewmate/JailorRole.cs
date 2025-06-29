using System.Globalization;
using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TMPro;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TownOfUs.Roles.Crewmate;

public sealed class JailorRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITouCrewRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "典狱长";
    public string RoleDescription => "关押并处决<color=#FF0000FF>伪装者</color>";
    public string RoleLongDescription => "在会议中处决恶人，但要避免误杀船员";
    public Color RoleColor => TownOfUsColors.Jailor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmatePower;
    public DoomableType DoomHintType => DoomableType.Relentless;
    public bool IsPowerCrew => Executes > 0; // Stop end game checks if the Jailor can still execute someone
    public override bool IsAffectedByComms => false;
    public CustomRoleConfiguration Configuration => new(this)
    {
        MaxRoleCount = 1,
        Icon = TouRoleIcons.Jailor,
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Impostor),
    };

    public int Executes { get; set; } = (int)OptionGroupSingleton<JailorOptions>.Instance.MaxExecutes;
    public PlayerControl Jailed => PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.GetModifier<JailedModifier>()?.JailorId == Player.PlayerId)!;

    private GameObject? executeButton;
    private TMP_Text? usesText;

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);

        Executes = (int)OptionGroupSingleton<JailorOptions>.Instance.MaxExecutes;
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        Clear();
    }

    public override void OnMeetingStart()
    {
        RoleStubs.RoleBehaviourOnMeetingStart(this);

        Clear();

        if (Player.HasDied()) return;

        if (Player.AmOwner)
        {
            if (Jailed!.HasDied())
                return;
            var title = $"<color=#{TownOfUsColors.Jailor.ToHtmlStringRGBA()}>典狱长反馈</color>";
            MiscUtils.AddFakeChat(Jailed.Data, title, "请在另一聊天框与被关押者交流。", false, true);
        }

        if (MeetingHud.Instance)
            AddMeetingButtons(MeetingHud.Instance);
    }

    public override void OnVotingComplete()
    {
        RoleStubs.RoleBehaviourOnVotingComplete(this);

        executeButton?.Destroy();
        usesText?.Destroy();
    }

    public void Clear()
    {
        executeButton?.Destroy();
        usesText?.Destroy();
    }

    public void LobbyStart()
    {
        Clear();
    }

    private void AddMeetingButtons(MeetingHud __instance)
    {
        if (Jailed == null || Jailed?.HasDied() == true) return;

        if (!Player.AmOwner) return;

        if (Executes <= 0 || Jailed?.HasDied() == true) return;

        if (Player.HasModifier<ImitatorCacheModifier>()) return;

        foreach (var voteArea in __instance.playerStates)
        {
            if (Jailed?.PlayerId == voteArea.TargetPlayerId)
            {
                // if (!(jailorRole.Jailed.IsLover() && PlayerControl.LocalPlayer.IsLover()))
                GenButton(voteArea);
            }
        }
    }


    private void GenButton(PlayerVoteArea voteArea)
    {
        var confirmButton = voteArea.Buttons.transform.GetChild(0).gameObject;

        var newButtonObj = Object.Instantiate(confirmButton, voteArea.transform);
        //newButtonObj.transform.position = confirmButton.transform.position - new Vector3(0.75f, 0f, -2.1f);
        newButtonObj.transform.position = confirmButton.transform.position - new Vector3(0.75f, 0f, 0f);
        newButtonObj.transform.localScale *= 0.8f;
        newButtonObj.layer = 5;
        newButtonObj.transform.parent = confirmButton.transform.parent.parent;

        executeButton = newButtonObj;

        var renderer = newButtonObj.GetComponent<SpriteRenderer>();
        renderer.sprite = TouAssets.ExecuteSprite.LoadAsset();

        var passive = newButtonObj.GetComponent<PassiveButton>();
        passive.OnClick = new Button.ButtonClickedEvent();
        passive.OnClick.AddListener(Execute());

        var usesTextObj = Object.Instantiate(voteArea.NameText, voteArea.transform);
        usesTextObj.transform.localPosition = new Vector3(-0.22f, 0.16f, newButtonObj.transform.position.z - 0.1f);
        usesTextObj.text = $"{Executes}";
        usesTextObj.transform.localScale = usesTextObj.transform.localScale * 0.65f;

        usesText = usesTextObj;
    }

    [HideFromIl2Cpp]
    private Action Execute()
    {
        void Listener()
        {
            if (Player.HasDied()) return;

            Clear();

            Executes--;

            if (Jailed.HasModifier<InvulnerabilityModifier>())
            {
                Coroutines.Start(MiscUtils.CoFlash(Color.red));

                var notif1 = Helpers.CreateAndShowNotification(
                    $"<b>{TownOfUsColors.Jailor.ToTextColor()}{Jailed.Data.PlayerName}无法被处决！他们拥有无敌效果！</color></b>", Color.white, spr: TouRoleIcons.Jailor.LoadAsset());

                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
            else
            {
                if (Jailed.Is(ModdedRoleTeams.Crewmate) && !(PlayerControl.LocalPlayer.TryGetModifier<AllianceGameModifier>(out var allyMod) && !allyMod.GetsPunished) && !(Jailed.TryGetModifier<AllianceGameModifier>(out var allyMod2) && !allyMod2.GetsPunished))
                {
                    Executes = 0;

                    CustomButtonSingleton<JailorJailButton>.Instance.ExecutedACrew = true;

                    Coroutines.Start(MiscUtils.CoFlash(Color.red));
                }
                else
                {
                    Coroutines.Start(MiscUtils.CoFlash(Color.green));
                }

                Player.RpcCustomMurder(Jailed, createDeadBody: false, teleportMurderer: false);
            }
        }

        return Listener;
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);
        if (PlayerControl.LocalPlayer.TryGetModifier<AllianceGameModifier>(out var allyMod) && !allyMod.GetsPunished)
        {
            stringB.AppendLine(CultureInfo.InvariantCulture, $"你可以处决船员。");
        }

        return stringB;
    }

    public string GetAdvancedDescription()
    {
        return "典狱长是一名船员强力型角色，可以关押其他玩家。在会议期间，典狱长可以选择处决被关押的玩家。（如果是效颦者变身的典狱长则无法处决）"
            + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("关押",
            "关押一名玩家。会议期间所有人都能看到被关押者。你可以通过私聊与被关押者交流。",
            TouCrewAssets.JailSprite),
        new("处决（会议）",
            "处决被关押的玩家。如果对方是船员，典狱长将失去关押能力。",
            TouAssets.ExecuteCleanSprite)
    ];
}
