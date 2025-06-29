using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Extensions;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class OracleRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "神谕者";
    public string RoleDescription => "让其他玩家坦白他们的身份";
    public string RoleLongDescription => "在你死亡时让其他玩家坦白身份";
    public Color RoleColor => TownOfUsColors.Oracle;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateProtective;
    public DoomableType DoomHintType => DoomableType.Insight;
    public override bool IsAffectedByComms => false;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Oracle,
        IntroSound = TouAudio.GuardianAngelSound,
    };

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public override void OnDeath(DeathReason reason)
    {
        RoleStubs.RoleBehaviourOnDeath(this, reason);

        RpcOracleConfess(Player);
    }

    public void ReportOnConfession()
    {
        if (!Player.AmOwner) return;

        var confessing = ModifierUtils.GetPlayersWithModifier<OracleConfessModifier>([HideFromIl2Cpp] (x) => x.Oracle == Player).FirstOrDefault();

        if (confessing == null) return;

        var report = BuildReport(confessing);

        var title = $"<color=#{TownOfUsColors.Oracle.ToHtmlStringRGBA()}>神谕者坦白</color>";
        MiscUtils.AddFakeChat(confessing.Data, title, report, false, true);
    }

    public static string BuildReport(PlayerControl player)
    {
        if (player.HasDied())
            return "你的坦白者未能存活，因此没有获得坦白信息";

        var allPlayers = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.HasDied() && x != PlayerControl.LocalPlayer && x != player).ToList();
        if (allPlayers.Count < 2)
            return "存活人数过少，无法获得坦白信息";

        var options = OptionGroupSingleton<OracleOptions>.Instance;

        var evilPlayers = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.HasDied() &&
            (x.IsImpostor() ||
            (x.Is(RoleAlignment.NeutralKilling) && options.ShowNeutralKillingAsEvil) ||
            (x.Is(RoleAlignment.NeutralEvil) && options.ShowNeutralEvilAsEvil) ||
            (x.Is(RoleAlignment.NeutralBenign) && options.ShowNeutralBenignAsEvil))).ToList();

        if (evilPlayers.Count == 0)
            return $"{player.GetDefaultAppearance().PlayerName} 坦白得知场上已无恶人！";

        allPlayers.Shuffle();
        evilPlayers.Shuffle();
        var secondPlayer = allPlayers[0];
        var firstTwoEvil = false;

        foreach (var evilPlayer in evilPlayers)
        {
            if (evilPlayer == player || evilPlayer == secondPlayer) firstTwoEvil = true;
        }

        if (firstTwoEvil)
        {
            var thirdPlayer = allPlayers[1];

            return $"{player.GetDefaultAppearance().PlayerName} 坦白得知自己、{secondPlayer.GetDefaultAppearance().PlayerName} 和/或 {thirdPlayer.GetDefaultAppearance().PlayerName} 是恶人！";
        }
        else
        {
            var thirdPlayer = evilPlayers[0];

            return $"{player.GetDefaultAppearance().PlayerName} 坦白得知自己、{secondPlayer.GetDefaultAppearance().PlayerName} 和/或 {thirdPlayer.GetDefaultAppearance().PlayerName} 是恶人！";
        }
    }

    [MethodRpc((uint)TownOfUsRpc.OracleConfess, SendImmediately = true)]
    public static void RpcOracleConfess(PlayerControl player)
    {
        var mod = ModifierUtils.GetActiveModifiers<OracleConfessModifier>(x => x.Oracle == player).FirstOrDefault();

        if (mod != null)
        {
            mod.ConfessToAll = true;
        }
    }

    [MethodRpc((uint)TownOfUsRpc.OracleBless, SendImmediately = true)]
    public static void RpcOracleBless(PlayerControl exiled)
    {
        // Logger<TownOfUsPlugin>.Message($"RpcOracleBless exiled '{exiled.Data.PlayerName}'");
        var mod = exiled.GetModifier<OracleBlessedModifier>();

        if (mod != null)
        {
            // Logger<TownOfUsPlugin>.Message($"RpcOracleBless exiled '{exiled.Data.PlayerName}' SavedFromExile");
            mod.SavedFromExile = true;
        }
    }
    public string GetAdvancedDescription()
    {
        return
            $"神谕者是一名船员保护型角色，可以让一名玩家坦白（当神谕者死亡时，以{OptionGroupSingleton<OracleOptions>.Instance.RevealAccuracyPercentage}%的准确率揭示其阵营），也可以保护一名玩家免受会议技能影响。"
               + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("佑护",
            $"佑护一名玩家，使其在会议中免受任何伤害。",
            TouCrewAssets.BlessSprite),
        new("坦白",
            $"让一名玩家在会议中坦白身份，展示3个可能的恶人（包括坦白者），并在神谕者死亡时以{OptionGroupSingleton<OracleOptions>.Instance.RevealAccuracyPercentage}%的准确率向所有人揭示其阵营。",
            TouCrewAssets.ConfessSprite),
    ];
}
