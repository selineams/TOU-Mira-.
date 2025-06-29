using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using System.Globalization;
using TownOfUs.Buttons.Neutral;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Roles.Neutral;

public sealed class MercenaryRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    public string RoleName => "雇佣兵";
    public string RoleDescription => "贿赂船员";
    public string RoleLongDescription => "守护船员，然后贿赂获胜者！";
    public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<WardenRole>());
    public Color RoleColor => TownOfUsColors.Mercenary;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralBenign;
    public DoomableType DoomHintType => DoomableType.Insight;
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.ToppatIntroSound,
        Icon = TouRoleIcons.Mercenary,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>(),
    };

    public static int BrideCost => (int)OptionGroupSingleton<MercenaryOptions>.Instance.BribeCost;

    public int Gold { get; set; }
    public bool CanBribe => Gold >= BrideCost;

    public override bool DidWin(GameOverReason gameOverReason)
    {
        var bribed = ModifierUtils.GetPlayersWithModifier<MercenaryBribedModifier>();

        return bribed.Any(x => x.Data.Role.DidWin(gameOverReason));
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);
        var players = ModifierUtils.GetPlayersWithModifier<MercenaryBribedModifier>();

        stringB.Append(CultureInfo.InvariantCulture, $"\n<b>金币:</b> {Gold}");

        var playerControls = players as PlayerControl[] ?? [.. players];
        if (playerControls.Length != 0)
        {
            stringB.Append($"\n<b>已贿赂:</b>");
        }

        foreach (var player in playerControls)
        {
            stringB.Append(CultureInfo.InvariantCulture, $"\n{player.Data.PlayerName}");
        }

        return stringB;
    }

    public void AddPayment()
    {
        Gold++;

        if (CanBribe)
        {
            CustomButtonSingleton<MercenaryBribeButton>.Instance.SetActive(true, this);
        }
    }

    public void Clear()
    {
        Gold = 0;

        CustomButtonSingleton<MercenaryGuardButton>.Instance.SetActive(true, this);
        CustomButtonSingleton<MercenaryBribeButton>.Instance.SetActive(false, this);
    }

    [MethodRpc((uint)TownOfUsRpc.Guarded, SendImmediately = true)]
    public static void RpcGuarded(PlayerControl player)
    {
        if (player.Data.Role is not MercenaryRole)
        {
            Logger<TownOfUsPlugin>.Error("RpcGuarded - Invalid mercenary");
            return;
        }

        if (!player.AmOwner) return;

        var mercenary = player.GetRole<MercenaryRole>();
        mercenary?.AddPayment();
    }
    public string GetAdvancedDescription()
    {
        return "雇佣兵是一名中立善良型角色，只能通过贿赂玩家来获胜，可以获得多个胜利条件。" 
            
               + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("守护",
            $"守护一名玩家可以为佣兵吸收对目标使用的能力，并获得金币用于贿赂。如果被贿赂的目标获胜，佣兵也会一同获胜。",
            TouNeutAssets.GuardSprite),
        new("贿赂",
            $"贿赂一名玩家可以让佣兵获得其胜利条件，前提是你有足够的金币。",
            TouNeutAssets.BribeSprite),
    ];
}
