using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TownOfUs.Modules;
using TownOfUs.Modules.Components;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class DetectiveRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "侧写师";
    public string RoleDescription => "调查案发现场，找出凶手";
    public string RoleLongDescription => "调查案发现场，然后检查玩家是否在现场。";
    public Color RoleColor => TownOfUsColors.Detective;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;
    public DoomableType DoomHintType => DoomableType.Insight;
    public override bool IsAffectedByComms => false;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Detective,
        IntroSound = TouAudio.QuestionSound,
    };

    public CrimeSceneComponent? InvestigatingScene { get; set; }

    [HideFromIl2Cpp]
    public List<byte> InvestigatedPlayers { get; init; } = new();

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        InvestigatingScene = null;
        InvestigatedPlayers.Clear();
    }

    public void LobbyStart()
    {
        InvestigatingScene = null;
        InvestigatedPlayers.Clear();

        CrimeSceneComponent.Clear();
    }

    public void ExaminePlayer(PlayerControl player)
    {
        if (InvestigatedPlayers.Contains(player.PlayerId))
        {
            Coroutines.Start(MiscUtils.CoFlash(Color.red));

            var deadPlayer = InvestigatingScene?.DeadPlayer!;

            var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.Detective.ToTextColor()}{player.Data.PlayerName}曾出现在{deadPlayer.Data.PlayerName}的死亡现场！\n他们可能是凶手或目击者。</b></color>", Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Detective.LoadAsset());
            notif1.Text.SetOutlineThickness(0.35f);
        }
        else
        {
            Coroutines.Start(MiscUtils.CoFlash(Color.green));
            var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.Detective.ToTextColor()}{player.Data.PlayerName}未出现在案发现场。</b></color>", Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Detective.LoadAsset());
            notif1.Text.SetOutlineThickness(0.35f);
        }
    }

    public void Report(byte deadPlayerId)
    {
        var areReportsEnabled = OptionGroupSingleton<DetectiveOptions>.Instance.DetectiveReportOn;

        if (!areReportsEnabled) return;

        var matches = GameHistory.KilledPlayers.Where(x => x.VictimId == deadPlayerId).ToArray();

        DeadPlayer? killer = null;

        if (matches.Length > 0)
            killer = matches[0];

        if (killer == null)
            return;

        var br = new BodyReport
        {
            Killer = MiscUtils.PlayerById(killer.KillerId),
            Reporter = Player,
            Body = MiscUtils.PlayerById(killer.VictimId),
            KillAge = (float)(DateTime.UtcNow - killer.KillTime).TotalMilliseconds,
        };

        var reportMsg = BodyReport.ParseDetectiveReport(br);

        if (string.IsNullOrWhiteSpace(reportMsg))
            return;

        // Send the message through chat only visible to the detective
        var title = $"<color=#{TownOfUsColors.Detective.ToHtmlStringRGBA()}>侧写师报告</color>";
        var reported = Player;
        if (br.Body != null) reported = br.Body;
        MiscUtils.AddFakeChat(reported.Data, title, reportMsg, false, true);
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    
    public string GetAdvancedDescription()
    {
        return "侧写师可以调查案发现场，并检查玩家是否在现场。如果玩家在现场会闪红光。" + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("调查",
            $"所有尸体都会生成案发现场。调查案发现场后检查玩家以发现线索。下次会议你会收到凶手身份的报告。",
            TouCrewAssets.InspectSprite),
        new("检查",
            $"调查案发现场后检查玩家。你会被告知该玩家是否在案发现场。",
            TouCrewAssets.ExamineSprite),
    ];
}
