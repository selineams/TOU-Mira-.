using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities.Extensions;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class DeputyRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITouCrewRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "副手";
    public string RoleDescription => "守护船员，抓住凶手";
    public string RoleLongDescription => "守护船员，在会议中射杀凶手！";
    public Color RoleColor => TownOfUsColors.Deputy;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateKilling;
    public DoomableType DoomHintType => DoomableType.Relentless;
    public override bool IsAffectedByComms => false;
    public bool IsPowerCrew => Killer; // Only stop end game checks if the deputy can actually kill someone
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Deputy,
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Impostor),
    };
    public static void OnRoundStart()
    {
        CustomButtonSingleton<CampButton>.Instance.Usable = true;
    }

    public PlayerControl? Killer { get; set; }

    private MeetingMenu meetingMenu;

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);

        if (Player.AmOwner)
        {
            meetingMenu = new MeetingMenu(
                this,
                ClickGuess,
                MeetingAbilityType.Click,
                TouAssets.ShootMeetingSprite,
                null!,
                IsExempt)
            {
                Position = new Vector3(-0.40f, 0f, -3f),
            };
        }
    }

    public override void OnMeetingStart()
    {
        RoleStubs.RoleBehaviourOnMeetingStart(this);

        if (Player.AmOwner)
        {
            meetingMenu.GenButtons(MeetingHud.Instance, Player.AmOwner && !Player.HasDied() && Killer != null && !Player.HasModifier<JailedModifier>());
        }
    }

    public override void OnVotingComplete()
    {
        RoleStubs.RoleBehaviourOnVotingComplete(this);

        if (Player.AmOwner)
        {
            meetingMenu.HideButtons();
        }

        Clear();
    }

    public override void OnDeath(DeathReason reason)
    {
        RoleStubs.RoleBehaviourOnDeath(this, reason);

        Clear();
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        Clear();

        if (Player.AmOwner)
        {
            meetingMenu?.Dispose();
            meetingMenu = null!;
        }
    }

    public void Clear()
    {
        var player = ModifierUtils.GetPlayersWithModifier<DeputyCampedModifier>(x => x.Deputy == PlayerControl.LocalPlayer).FirstOrDefault();

        if (player != null && Player.AmOwner)
        {
            player.RpcRemoveModifier<DeputyCampedModifier>();
        }
    }

    public void ClickGuess(PlayerVoteArea voteArea, MeetingHud __)
    {
        var target = GameData.Instance.GetPlayerById(voteArea.TargetPlayerId).Object;
        var role = Player.GetRole<DeputyRole>()!;

        if (role.Killer == target && !target.HasModifier<InvulnerabilityModifier>())
        {
            Player.RpcCustomMurder(target, createDeadBody: false, teleportMurderer: false);
        }
        else
        {
            var title = $"<color=#{TownOfUsColors.Deputy.ToHtmlStringRGBA()}>副手反馈</color>";
            MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, title, "你射偏了！对方不是凶手或拥有无敌效果。", false, true);
            var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.Deputy.ToTextColor()}你射偏了！对方不是凶手或拥有无敌效果。</b></color>", Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Deputy.LoadAsset());
            notif1.Text.SetOutlineThickness(0.35f);
        }

        if (Player.AmOwner)
        {
            meetingMenu?.HideButtons();
        }

        Clear();
    }

    public bool IsExempt(PlayerVoteArea voteArea)
    {
        return voteArea?.TargetPlayerId == Player.PlayerId || Player.Data.IsDead || voteArea!.AmDead || voteArea.GetPlayer()?.HasModifier<JailedModifier>() == true;
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    
    public string GetAdvancedDescription()
    {
        return "副手是一名船员击杀型角色，可以守护其他玩家。当被守护的玩家死亡时，副手会收到提示。在接下来的会议中，副手可以尝试射杀凶手，若成功则凶手死亡，否则无事发生。" + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("守护",
            $"守护一名玩家，当其死亡时你会收到提示。之后你可以尝试射杀凶手，若成功凶手死亡，否则无事发生。",
            TouCrewAssets.CampButtonSprite),
    ];
}
