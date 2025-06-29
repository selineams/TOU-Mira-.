using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using Reactor.Utilities.Extensions;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class TrapperRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "陷阱师";
    public string RoleDescription => "当场抓住杀手";
    public string RoleLongDescription => "在地图上放置陷阱，揭示其中玩家的身份";
    public Color RoleColor => TownOfUsColors.Trapper;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;
    public DoomableType DoomHintType => DoomableType.Insight;
    public override bool IsAffectedByComms => false;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Trapper,
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Tracker),
    };

    [HideFromIl2Cpp]
    public List<RoleBehaviour> TrappedPlayers { get; set; } = new();

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        Clear();
    }

    public void LobbyStart()
    {
        Clear();
    }

    public void Clear()
    {
        TrappedPlayers.Clear();
        Trap.Clear();
    }

    public void Report()
    {
        // Logger<TownOfUsPlugin>.Error($"TrapperRole.Report");
        if (!Player.AmOwner) return;

        var minAmountOfPlayersInTrap = OptionGroupSingleton<TrapperOptions>.Instance.MinAmountOfPlayersInTrap;
        var msg = "没有玩家进入你的陷阱";

        if (TrappedPlayers.Count < minAmountOfPlayersInTrap)
        {
            msg = "触发你陷阱的玩家数量不足";
        }
        else if (TrappedPlayers.Count != 0)
        {
            var message = new StringBuilder("被你陷阱捕获的身份：\n");

            TrappedPlayers.Shuffle();

            foreach (var role in TrappedPlayers)
            {
                message.Append(TownOfUsPlugin.Culture, $"{role.NiceName}, ");
            }

            message = message.Remove(message.Length - 2, 2);

            var finalMessage = message.ToString();

            if (string.IsNullOrWhiteSpace(finalMessage))
                return;

            msg = finalMessage;
        }

        var title = $"<color=#{TownOfUsColors.Trapper.ToHtmlStringRGBA()}>陷阱师报告</color>";
        MiscUtils.AddFakeChat(Player.Data, title, msg, false, true);
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return "陷阱师是一名船员调查型角色，可以在地图上放置陷阱。 " +
               "如果有足够多的玩家进入并停留足够时间， " +
               "下一次会议时你会获得这些玩家的身份列表（顺序随机）。" +
               MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("放置陷阱",
            "放置一个陷阱。根据设置，陷阱可能会持续整局游戏或在会议后重置。",
            TouCrewAssets.TrapSprite)
    ];
}
