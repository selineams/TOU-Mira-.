using System.Globalization;
using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TownOfUs.Modifiers.Game.Alliance;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class SnitchRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "告密者";
    public string RoleDescription => "找出<color=#FF0000FF>伪装者</color>！";
    public string RoleLongDescription => CompletedAllTasks ? "找出伪装者！" : "完成所有任务以发现伪装者。";
    public Color RoleColor => TownOfUsColors.Snitch;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;
    public DoomableType DoomHintType => DoomableType.Insight;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Snitch,
        IntroSound = TouAudio.ToppatIntroSound,
    };

    private Dictionary<byte, ArrowBehaviour>? _snitchArrows;
    public ArrowBehaviour? SnitchRevealArrow { get; private set; }
    public bool CompletedAllTasks { get; private set; }
    public bool OnLastTask { get; private set; }

    public void CheckTaskRequirements()
    {
        var completedTasks = Player.myTasks.ToArray().Count(t => t.IsComplete);

        OnLastTask = (Player.myTasks.Count - completedTasks) <= (int)OptionGroupSingleton<SnitchOptions>.Instance.TaskRemainingWhenRevealed;

        if (IsTargetOfSnitch(PlayerControl.LocalPlayer) && OnLastTask)
        {
            CreateRevealingArrow();
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Snitch, alpha: 0.5f));
            var text = "告密者快要发现你了！";
            if (Player.HasModifier<EgotistModifier>()) text = "告密者是营己徒，会帮助你推翻船员！";
            var notif1 = Helpers.CreateAndShowNotification(
                $"<b>{TownOfUsColors.Snitch.ToTextColor()}{text}</color></b>", Color.white, spr: TouRoleIcons.Snitch.LoadAsset());

            notif1.Text.SetOutlineThickness(0.35f);
            notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
        }

        CompletedAllTasks = completedTasks == Player.myTasks.Count;

        if (OnLastTask && Player.AmOwner && !CompletedAllTasks)
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Snitch, alpha: 0.5f));
            var text = "伪装者已经知道你的位置！";
            if (Player.HasModifier<EgotistModifier>()) text = "伪装者已经知道你的位置，并且知道你是营己徒！";
            var notif1 = Helpers.CreateAndShowNotification(
                $"<b>{TownOfUsColors.Snitch.ToTextColor()}{text}</color></b>", Color.white, spr: TouRoleIcons.Snitch.LoadAsset());

            notif1.Text.SetOutlineThickness(0.35f);
            notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
        }

        if (CompletedAllTasks && IsTargetOfSnitch(PlayerControl.LocalPlayer))
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Snitch, alpha: 0.5f));
            var text = "告密者现在已经知道你的身份！";
            if (Player.HasModifier<EgotistModifier>()) text = "告密者现在可以作为营己徒帮助你！";
            var notif1 = Helpers.CreateAndShowNotification(
                $"<b>{TownOfUsColors.Snitch.ToTextColor()}{text}</color></b>", Color.white, spr: TouRoleIcons.Snitch.LoadAsset());

            notif1.Text.SetOutlineThickness(0.35f);
            notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
        }

        if (CompletedAllTasks && Player.AmOwner)
        {
            CreateSnitchArrows();
            var text = "你已经发现了伪装者！";
            if (Player.HasModifier<EgotistModifier>()) text = "你已经发现了伪装者，他们可以帮助你达成胜利条件！";
            var notif1 = Helpers.CreateAndShowNotification(
                $"<b>{TownOfUsColors.Snitch.ToTextColor()}{text}</color></b>", Color.white, spr: TouRoleIcons.Snitch.LoadAsset());

            notif1.Text.SetOutlineThickness(0.35f);
            notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
        }
    }

    public static bool IsTargetOfSnitch(PlayerControl player)
    {
        if (player == null || player.Data == null || player.Data.Role == null)
        {
            return false;
        }

        return (player.IsImpostor() && !player.IsTraitor()) || (player.IsTraitor() && OptionGroupSingleton<SnitchOptions>.Instance.SnitchSeesTraitor) || (player.Is(RoleAlignment.NeutralKilling) && OptionGroupSingleton<SnitchOptions>.Instance.SnitchNeutralRoles);
    }

    public static bool SnitchVisibilityFlag(PlayerControl player, bool showRole = false)
    {
        var snitchRevealed = PlayerControl.LocalPlayer.Data.Role is SnitchRole snitch && snitch.CompletedAllTasks;
        var showSnitch = IsTargetOfSnitch(PlayerControl.LocalPlayer) && player.Data.Role is SnitchRole snitch2 && snitch2.OnLastTask;

        if (MeetingHud.Instance && !OptionGroupSingleton<SnitchOptions>.Instance.SnitchSeesImpostorsMeetings)
        {
            snitchRevealed = false;
        }

        return (snitchRevealed && IsTargetOfSnitch(player) && !showRole) || showSnitch;
    }

    public override void OnDeath(DeathReason reason)
    {
        RoleStubs.RoleBehaviourOnDeath(this, reason);

        ClearArrows();
    }

    public void RemoveArrowForPlayer(byte playerId)
    {
        if (_snitchArrows != null && _snitchArrows.TryGetValue(playerId, out var arrow))
        {
            arrow.gameObject.Destroy();
            _snitchArrows.Remove(playerId);
        }
    }

    public void ClearArrows()
    {
        if (_snitchArrows != null && _snitchArrows.Count > 0)
        {
            _snitchArrows.ToList().ForEach(arrow => arrow.Value.gameObject.Destroy());
            _snitchArrows.Clear();
        }

        if (SnitchRevealArrow != null)
        {
            SnitchRevealArrow.gameObject.Destroy();
        }
    }

    private void CreateRevealingArrow()
    {
        if (SnitchRevealArrow != null)
        {
            return;
        }

        PlayerNameColor.Set(Player);
        Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Snitch, alpha: 0.5f));
        SnitchRevealArrow = MiscUtils.CreateArrow(Player.transform, TownOfUsColors.Snitch);
    }

    private void CreateSnitchArrows()
    {
        if (_snitchArrows != null)
        {
            return;
        }

        Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Snitch, alpha: 0.5f));
        _snitchArrows = new();
        var imps = Helpers.GetAlivePlayers().Where(plr => plr.Data.Role.IsImpostor && !plr.IsTraitor());
        var traitor = Helpers.GetAlivePlayers().FirstOrDefault(plr => plr.IsTraitor());
        imps.ToList().ForEach(imp =>
        {
            _snitchArrows.Add(imp.PlayerId, MiscUtils.CreateArrow(imp.transform, TownOfUsColors.Impostor));
            PlayerNameColor.Set(imp);
        });

        if (OptionGroupSingleton<SnitchOptions>.Instance.SnitchSeesTraitor && traitor != null)
        {
            _snitchArrows.Add(traitor.PlayerId, MiscUtils.CreateArrow(traitor.transform, TownOfUsColors.Impostor));
            PlayerNameColor.Set(traitor);
        }

        if (OptionGroupSingleton<SnitchOptions>.Instance.SnitchNeutralRoles)
        {
            var neutrals = MiscUtils.GetRoles(RoleAlignment.NeutralKilling).Where(role => !role.Player.Data.IsDead && !role.Player.Data.Disconnected);
            neutrals.ToList().ForEach(neutral =>
            {
                _snitchArrows.Add(neutral.Player.PlayerId, MiscUtils.CreateArrow(neutral.Player.transform, TownOfUsColors.Neutral));
                PlayerNameColor.Set(neutral.Player);
            });
        }
    }
    public void AddSnitchTraitorArrows()
    {
        var completedTasks = Player.myTasks.ToArray().Count(t => t.IsComplete);

        OnLastTask = (Player.myTasks.Count - completedTasks) <= (int)OptionGroupSingleton<SnitchOptions>.Instance.TaskRemainingWhenRevealed;

        if (PlayerControl.LocalPlayer.IsTraitor() && OptionGroupSingleton<SnitchOptions>.Instance.SnitchSeesTraitor && OnLastTask)
        {
            CreateRevealingArrow();
        }

        CompletedAllTasks = completedTasks == Player.myTasks.Count;

        if (CompletedAllTasks && Player.AmOwner)
        {
            var traitor = Helpers.GetAlivePlayers().FirstOrDefault(plr => plr.IsTraitor());
            if (_snitchArrows == null || traitor == null || (_snitchArrows.TryGetValue(traitor.PlayerId, out var arrow) && arrow != null))
            {
                return;
            }
            if (OptionGroupSingleton<SnitchOptions>.Instance.SnitchSeesTraitor && traitor != null)
            {
                _snitchArrows.Add(traitor.PlayerId, MiscUtils.CreateArrow(traitor.transform, TownOfUsColors.Impostor));
                PlayerNameColor.Set(traitor);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Player == null || Player.Data.Role is not SnitchRole) return;

        if (SnitchRevealArrow != null && SnitchRevealArrow.target != SnitchRevealArrow.transform.parent.position)
        {
            SnitchRevealArrow.target = Player.transform.position;
        }

        if (_snitchArrows != null && _snitchArrows.Count > 0 && Player.AmOwner)
        {
            _snitchArrows.ToList().ForEach(arrow => arrow.Value.target = arrow.Value.transform.parent.position);
        }
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var alignment = RoleAlignment.ToDisplayString().Replace("船员", "<color=#68ACF4FF>船员");

        var stringB = new StringBuilder();
        stringB.AppendLine(CultureInfo.InvariantCulture, $"{RoleColor.ToTextColor()}你是<b> {RoleName}。</b></color>");
        stringB.AppendLine(CultureInfo.InvariantCulture, $"<size=60%>阵营: <b>{alignment}</color></b></size>");
        stringB.Append("<size=70%>");
        var desc = RoleLongDescription;
        if (PlayerControl.LocalPlayer.HasModifier<EgotistModifier>()) desc = CompletedAllTasks ? "帮助伪装者！" : "完成所有任务以发现并帮助伪装者。";
        stringB.AppendLine(CultureInfo.InvariantCulture, $"{desc}");
    
        return stringB;
    }
    
    public string GetAdvancedDescription()
    {
        return
            "告密者是一名船员调查型角色，通过完成所有任务可以发现伪装者。 " +
            "完成所有任务后，伪装者会被箭头和红名标记。"
            + MiscUtils.AppendOptionsText(GetType());
    }
}
