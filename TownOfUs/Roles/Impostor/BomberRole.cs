using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs.Events.TouEvents;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Impostor;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Impostor;

public sealed class BomberRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    public string RoleName => "爆破手";
    public string RoleDescription => "布置炸弹，一次性击杀多名船员";
    public string RoleLongDescription => "布置炸弹，可一次性击杀多名船员";
    public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<TrapperRole>());
    public Color RoleColor => TownOfUsColors.Impostor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorKilling;
    public DoomableType DoomHintType => DoomableType.Relentless;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Bomber,
        CanUseVent = OptionGroupSingleton<BomberOptions>.Instance.BomberVent,
    };

    [HideFromIl2Cpp]
    public Bomb? Bomb { get; set; }

    [MethodRpc((uint)TownOfUsRpc.PlantBomb, SendImmediately = true)]
    public static void RpcPlantBomb(PlayerControl player, Vector2 position)
    {
        if (player.Data.Role is not BomberRole role)
        {
            Logger<TownOfUsPlugin>.Error("RpcPlantBomb - Invalid bomber");
            return;
        }

        var touAbilityEvent = new TouAbilityEvent(AbilityType.BomberPlant, player);
        MiraEventManager.InvokeEvent(touAbilityEvent);

        if (player.AmOwner)
        {
            role.Bomb = Bomb.CreateBomb(player, position);
        }
        else if (OptionGroupSingleton<BomberOptions>.Instance.AllImpsSeeBomb && PlayerControl.LocalPlayer.IsImpostor())
        {
            Bomb.BombShowTeammate(player, position);
        }
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    public string GetAdvancedDescription()
    {
        return $"爆破手是一名伪装者击杀型角色，可以在地图上布置炸弹，{OptionGroupSingleton<BomberOptions>.Instance.DetonateDelay}秒后爆炸。" + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } =
        [
            new("布置", 
                $"布置一个炸弹，显示其爆炸范围，最多可击杀{(int)OptionGroupSingleton<BomberOptions>.Instance.MaxKillsInDetonation}名玩家。",
                TouImpAssets.PlaceSprite)
        ];
}
