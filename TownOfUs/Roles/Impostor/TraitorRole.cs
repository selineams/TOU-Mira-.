using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modules.Wiki;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Impostor;

public sealed class TraitorRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ISpawnChange
{
    public string RoleName => "背叛者";
    public string RoleDescription => "背叛船员！";
    public string RoleLongDescription => "背叛船员！";
    public Color RoleColor => TownOfUsColors.Impostor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorKilling;
    public bool NoSpawn => true;
    public DoomableType DoomHintType => DoomableType.Trickster;
    public CustomRoleConfiguration Configuration => new(this)
    {
        MaxRoleCount = 1,
        Icon = TouRoleIcons.Traitor,
    };

    [HideFromIl2Cpp] public List<RoleBehaviour> ChosenRoles { get; } = [];
    public RoleBehaviour? RandomRole { get; set; }
    public RoleBehaviour? SelectedRole { get; set; }

    public void Clear()
    {
        ChosenRoles.Clear();
        SelectedRole = null;
    }

    public void UpdateRole()
    {
        if (!SelectedRole) return;

        var roleType = RoleId.Get(SelectedRole!.GetType());
        Player.RpcChangeRole(roleType, false);
        Player.RpcAddModifier<TraitorCacheModifier>();
        SelectedRole = null;
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    public string GetAdvancedDescription()
    {
        return "背叛者是一名伪装者击杀型角色，会在会议后满足条件时生成。背叛者不会成为市长，且必须是船员。背叛者的目标是为已死伪装者赢得游戏并消灭船员，同时可以变更为更强力的角色。"
               + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("变更角色",
            $"背叛者可以从提供的角色卡中选择变更，或随机抽取。选择后将一直保持该角色直到死亡，但仍需被猜中为背叛者。",
            TouImpAssets.TraitorSelect),
    ];
}
