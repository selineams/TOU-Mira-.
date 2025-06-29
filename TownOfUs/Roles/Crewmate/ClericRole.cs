using System.Text;
using Il2CppInterop.Runtime.Attributes;
using AmongUs.GameOptions;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;
using Reactor.Networking.Attributes;
using Reactor.Utilities;

namespace TownOfUs.Roles.Crewmate;

public sealed class ClericRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "牧师";
    public string RoleDescription => "拯救船员";
    public string RoleLongDescription => "为船员加护盾并净化负面效果";
    public Color RoleColor => TownOfUsColors.Cleric;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateProtective;
    public DoomableType DoomHintType => DoomableType.Protective;
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Scientist),
        Icon = TouRoleIcons.Cleric,
    };
    public override bool IsAffectedByComms => false;
    [MethodRpc((uint)TownOfUsRpc.ClericBarrierAttacked, SendImmediately = true)]
    public static void RpcClericBarrierAttacked(PlayerControl cleric, PlayerControl source, PlayerControl shielded)
    {
        if (cleric.Data.Role is not ClericRole)
        {
            Logger<TownOfUsPlugin>.Error("RpcClericBarrierAttacked - Invalid cleric");
            return;
        }

        if (PlayerControl.LocalPlayer.PlayerId == source.PlayerId || (PlayerControl.LocalPlayer.PlayerId == cleric.PlayerId && OptionGroupSingleton<ClericOptions>.Instance.AttackNotif))
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Cleric));
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return "牧师是一名船员保护型角色，可以通过净化负面效果和为船员加护盾来保护他们。" + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("护盾",
            $"使一名船员免受互动影响。护盾持续{OptionGroupSingleton<ClericOptions>.Instance.BarrierCooldown}秒。",
            TouCrewAssets.BarrierSprite),
        new("净化",
            $"移除玩家身上的所有负面效果。（涂油、黑入、感染、勒索、致盲、闪光、催眠）",
            TouCrewAssets.CleanseSprite)
    ];
}
