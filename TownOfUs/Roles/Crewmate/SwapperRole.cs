using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class SwapperRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITouCrewRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "换票师";
    public string RoleDescription => "交换票数拯救船员！";
    public string RoleLongDescription => "在会议期间将一名玩家的票数转移到另一名玩家身上";
    public Color RoleColor => TownOfUsColors.Swapper;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmatePower;
    public DoomableType DoomHintType => DoomableType.Trickster;
    public bool IsPowerCrew => true; // Always disable end game checks because a swapper can still screw people over
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Swapper,
        MaxRoleCount = 1,
        IntroSound = TouAudio.TimeLordIntroSound,
    };

    public PlayerVoteArea? Swap1 { get; set; }
    public PlayerVoteArea? Swap2 { get; set; }

    private MeetingMenu meetingMenu;

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);

        if (Player.AmOwner)
        {
            meetingMenu = new MeetingMenu(this, SetActive, MeetingAbilityType.Toggle, TouAssets.SwapActive, TouAssets.SwapInactive, IsExempt)
            {
                Position = new Vector3(-0.40f, 0f, -3f),
            };
        }

        if (!OptionGroupSingleton<SwapperOptions>.Instance.CanButton)
        {
            player.RemainingEmergencies = 0;
        }
    }

    public override void OnMeetingStart()
    {
        RoleStubs.RoleBehaviourOnMeetingStart(this);

        if (Player.AmOwner)
        {
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

    private static bool IsExempt(PlayerVoteArea voteArea)
    {
        var player = GameData.Instance.GetPlayerById(voteArea.TargetPlayerId)?.Object;

        return !player || !player?.Data || player!.Data.Disconnected || player.Data.IsDead || player.HasModifier<JailedModifier>();
    }

    private void SetActive(PlayerVoteArea voteArea, MeetingHud __instance)
    {
        if (__instance.state == MeetingHud.VoteStates.Discussion || IsExempt(voteArea))
        {
            return;
        }

        if (!Swap1)
        {
            Swap1 = voteArea;
            meetingMenu.Actives[voteArea.TargetPlayerId] = true;
        }
        else if (!Swap2)
        {
            Swap2 = voteArea;
            meetingMenu.Actives[voteArea.TargetPlayerId] = true;
        }
        else if (Swap1 == voteArea)
        {
            meetingMenu.Actives[Swap1!.TargetPlayerId] = false;
            Swap1 = null;
        }
        else if (Swap2 == voteArea)
        {
            meetingMenu.Actives[Swap2!.TargetPlayerId] = false;
            Swap2 = null;
        }
        else
        {
            meetingMenu.Actives[Swap1!.TargetPlayerId] = false;
            Swap1 = Swap2;
            Swap2 = voteArea;
            meetingMenu.Actives[voteArea.TargetPlayerId] = !meetingMenu.Actives[voteArea.TargetPlayerId];
        }

        RpcSyncSwaps(Player, Swap1?.TargetPlayerId ?? 255, Swap2?.TargetPlayerId ?? 255);
    }

    [MethodRpc((uint)TownOfUsRpc.SetSwaps)]
    public static void RpcSyncSwaps(PlayerControl swapper, byte swap1, byte swap2)
    {
        var swapperRole = swapper.Data?.Role as SwapperRole;
        var areas = MeetingHud.Instance.playerStates.ToList();
        swapperRole!.Swap1 = areas.Find(x => x.TargetPlayerId == swap1);
        swapperRole.Swap2 = areas.Find(x => x.TargetPlayerId == swap2);
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    
    public string GetAdvancedDescription()
    {
        return
            "换票师是一名船员强力型角色，可以在会议中交换两名玩家的票数。" +
            "被交换的两名玩家的投票区会互换，票数也会互换。" +
            "如果1号玩家原本票数最多，被你和2号玩家交换后，2号玩家将被放逐。 "
            + MiscUtils.AppendOptionsText(GetType());
    }
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("交换（会议）",
            "选择两名玩家交换票数，会议结束时他们的位置会互换！",
            TouAssets.SwapActive),
    ];
}
