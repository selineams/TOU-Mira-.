using System.Globalization;
using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Utilities.Extensions;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game.Alliance;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class PoliticianRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITouCrewRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "政治家";
    public string RoleDescription => "竞选成为市长！";
    public string RoleLongDescription => "宣传你的竞选活动，成为市长！";
    public Color RoleColor => TownOfUsColors.Politician;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmatePower;
    public DoomableType DoomHintType => DoomableType.Trickster;
    public override bool IsAffectedByComms => false;
    public bool IsPowerCrew => true;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Politician,
        IntroSound = TouAudio.MayorRevealSound,
        MaxRoleCount = 1,
    };

    public bool CanCampaign { get; set; } = true;

    private MeetingMenu meetingMenu;

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);

        if (Player.AmOwner)
        {
            meetingMenu = new MeetingMenu(
                this,
                Click,
                MeetingAbilityType.Click,
                TouAssets.RevealButtonSprite,
                null!,
                IsExempt)
            {
                Position = new Vector3(-0.35f, 0f, -3f),
            };
        }
    }

    public override void OnMeetingStart()
    {
        RoleStubs.RoleBehaviourOnMeetingStart(this);

        CanCampaign = true;

        if (Player.AmOwner)
        {
            // Logger<TownOfUsPlugin>.Message($"PoliticianRole.OnMeetingStart '{Player.Data.PlayerName}' {Player.AmOwner && !Player.HasDied() && !Player.HasModifier<JailedModifier>()}");
            meetingMenu.GenButtons(MeetingHud.Instance, Player.AmOwner && !Player.HasDied() && !Player.HasModifier<JailedModifier>());
        }
    }

    public override void OnVotingComplete()
    {
        RoleStubs.RoleBehaviourOnVotingComplete(this);

        if (Player.AmOwner)
        {
            meetingMenu.HideButtons();
        }
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        if (Player.AmOwner)
        {
            meetingMenu?.Dispose();
            meetingMenu = null!;
        }
    }

    public void Click(PlayerVoteArea voteArea, MeetingHud __)
    {
        if (!Player.AmOwner) return;

        meetingMenu.HideButtons();

        var aliveCrew = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.HasDied() && x.IsCrewmate());
        var aliveCampaigned = aliveCrew.Count(x => x.HasModifier<PoliticianCampaignedModifier>());
        var hasMajority = aliveCampaigned >= Math.Max(aliveCrew.Count() / 2 - 1, 1); // minus one to account for politician, max of at least 1 crewmate campaigned
        if (!aliveCrew.Any(x => x.Data.Role is not PoliticianRole)) hasMajority = true; // if all crew are dead, politician can reveal

        if (hasMajority)
        {
            Player.RpcChangeRole(RoleId.Get<MayorRole>());
            if (Player.HasModifier<ToBecomeTraitorModifier>())
            {
                Player.GetModifier<ToBecomeTraitorModifier>()!.Clear();
            }
        }
        else
        {
            var text = "你需要拉拢更多船员！本次会议你不能再次现身。";
            if (OptionGroupSingleton<PoliticianOptions>.Instance.PreventCampaign)
            {
                CanCampaign = false;
                text = "你需要拉拢更多船员！不过你下回合不能拉票。";
            }
            var title = $"<color=#{TownOfUsColors.Mayor.ToHtmlStringRGBA()}>政客反馈</color>";
            MiscUtils.AddFakeChat(Player.Data, title, text, false, true);
        }
    }

    public bool IsExempt(PlayerVoteArea voteArea)
    {
        return voteArea?.TargetPlayerId != Player.PlayerId;
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);
        if (PlayerControl.LocalPlayer.HasModifier<EgotistModifier>())
        {
            stringB.AppendLine(CultureInfo.InvariantCulture, $"<b>现身后伪装者会知道你的真实意图。</b>");
        }

        return stringB;
    }
    public string GetAdvancedDescription()
    {
        return "政客是一名船员强力型角色，只要拉拢到至少一半船员支持，就能向全体公开身份并成为市长。"
               + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("拉票",
            "给一名玩家发放选票，只有船员身份的玩家才会对你有用。",
            TouCrewAssets.CampaignButtonSprite),
        new("现身（会议）",
            "如果你现身且有超过一半的船员被你拉票（或没有其他船员存活），你将成为市长！否则本回合你的能力会失败，且" + (OptionGroupSingleton<PoliticianOptions>.Instance.PreventCampaign ? "下回合不能" : "下回合可以") + "继续拉票。",
            TouAssets.RevealCleanSprite),
    ];
}
