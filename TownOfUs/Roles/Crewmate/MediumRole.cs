using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules.Wiki;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class MediumRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "招魂师";
    public string RoleDescription => "观察幽灵的动向";
    public string RoleLongDescription => "跟随幽灵以获取线索";
    public Color RoleColor => TownOfUsColors.Medium;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateSupport;
    public DoomableType DoomHintType => DoomableType.Death;
    public override bool IsAffectedByComms => false;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Medium,
        IntroSound = TouAudio.MediumIntroSound,
    };

    [HideFromIl2Cpp]
    public List<MediatedModifier> MediatedPlayers { get; private set; } = new();

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        MediatedPlayers.ForEach(mod => mod.Player?.GetModifierComponent()?.RemoveModifier(mod));
    }

    [MethodRpc((uint)TownOfUsRpc.Mediate, LocalHandling = RpcLocalHandling.Before, SendImmediately = true)]
    public static void RpcMediate(PlayerControl source, PlayerControl target)
    {
        if ((!source.AmOwner && !target.AmOwner) || (source.Data.Role is not MediumRole && !target.Data.IsDead))
        {
            return;
        }

        var modifier = new MediatedModifier(source.PlayerId);
        target.GetModifierComponent()?.AddModifier(modifier);
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return "招魂师是一名船员支援型角色，可以招魂，每次可看到一名幽灵。招魂师和幽灵会互相显示箭头，随时指示对方的位置。"
            + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("招魂",
            "与死者交流，可能会获得指向凶手的线索。",
            TouCrewAssets.MediateSprite)    
    ];
}
