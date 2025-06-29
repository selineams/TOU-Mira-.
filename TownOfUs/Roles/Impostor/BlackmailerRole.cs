using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using TownOfUs.Events.TouEvents;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Impostor;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Impostor;

public sealed class BlackmailerRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "勒索者";
    public string RoleDescription => "在会议中让船员沉默";
    public string RoleLongDescription => "让一名船员在下次会议中无法发言";
    public Color RoleColor => TownOfUsColors.Impostor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorSupport;
    public DoomableType DoomHintType => DoomableType.Insight;
    public CustomRoleConfiguration Configuration => new(this)
    {
        UseVanillaKillButton = true,
        Icon = TouRoleIcons.Blackmailer,
    };
    public void FixedUpdate()
    {
        if (Player == null || Player.Data.Role is not JanitorRole || Player.HasDied() || !Player.AmOwner || MeetingHud.Instance || (!HudManager.Instance.UseButton.isActiveAndEnabled && !HudManager.Instance.PetButton.isActiveAndEnabled)) return;
        HudManager.Instance.KillButton.ToggleVisible(OptionGroupSingleton<BlackmailerOptions>.Instance.BlackmailerKill || (Player != null && Player.GetModifiers<BaseModifier>().Any(x => x is ICachedRole)) || (Player != null && MiscUtils.ImpAliveCount == 1));
    }

    [MethodRpc((uint)TownOfUsRpc.Blackmail, LocalHandling = RpcLocalHandling.Before, SendImmediately = true)]
    public static void RpcBlackmail(PlayerControl source, PlayerControl target)
    {
        var existingBmed = PlayerControl.AllPlayerControls.ToArray()
            .FirstOrDefault(x => x.GetModifier<BlackmailedModifier>()?.BlackMailerId == source.PlayerId);

        existingBmed?.RemoveModifier<BlackmailedModifier>();

        var modifier = new BlackmailedModifier(source.PlayerId);
        target.AddModifier(modifier);
        var touAbilityEvent = new TouAbilityEvent(AbilityType.BlackmailerBlackmail, source, target);
        MiraEventManager.InvokeEvent(touAbilityEvent);
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return $"勒索者是一名伪装者辅助型角色，可以让一名玩家在下次会议中无法发言（以<color=#2A1119>M</color>标记）。" + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities => [
        new("勒索",
            "让一名玩家在下次会议中无法发言。" +
            "他们将无法说话，且在存活人数大于等于设定值前无法投票。" +
            "是否公开勒索状态可由设置决定。",
            TouImpAssets.BlackmailSprite),
    ];
}
