using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules.Wiki;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class LookoutRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "观测者";
    public string RoleDescription => "时刻保持警惕";
    public string RoleLongDescription => "观察其他船员，了解与其互动的角色";
    public Color RoleColor => TownOfUsColors.Lookout;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;
    public DoomableType DoomHintType => DoomableType.Hunter;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Lookout,
        IntroSound = TouAudio.QuestionSound,
    };
    public override bool IsAffectedByComms => false;

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    [MethodRpc((uint)TownOfUsRpc.LookoutSeePlayer, SendImmediately = true)]
    public static void RpcSeePlayer(PlayerControl target, PlayerControl source)
    {
        if (!target.TryGetModifier<LookoutWatchedModifier>(out var mod))
        {
            Logger<TownOfUsPlugin>.Error("无被观测的玩家");
            return;
        }

        var role = source.Data.Role;

        var cachedMod = source.GetModifiers<BaseModifier>().FirstOrDefault(x => x is ICachedRole) as ICachedRole;
        if (cachedMod != null)
        {
            role = cachedMod.CachedRole;
        }
        // Prevents duplicate role entries
        if (!mod.SeenPlayers.Contains(role)) mod.SeenPlayers.Add(role);

    }

    public string GetAdvancedDescription() 
    {
        return "观测者是一名船员调查型角色，可以在回合中观测其他玩家。会议期间你会看到所有与被观测者互动过的角色。"
            + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("观测",
            "观测一名或多名玩家，下次会议你会知道哪些玩家与被观测者有过互动。",
            TouCrewAssets.WatchSprite)
    ];
}
