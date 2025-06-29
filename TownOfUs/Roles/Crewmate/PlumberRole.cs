using System.Collections;
using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Events;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs.Events.TouEvents;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class PlumberRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "水管工";
    public string RoleDescription => "把老鼠赶出下水道";
    public string RoleLongDescription => "冲洗通风口驱逐潜伏者，\n并在下一回合封锁通风口阻止其使用";
    public Color RoleColor => TownOfUsColors.Plumber;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateSupport;
    public DoomableType DoomHintType => DoomableType.Trickster;
    public override bool IsAffectedByComms => false;

    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Engineer),
        Icon = TouRoleIcons.Plumber,
    };

    [HideFromIl2Cpp]
    public List<int> FutureBlocks { get; set; } = [];

    [HideFromIl2Cpp]
    public List<int> VentsBlocked { get; set; } = [];

    [HideFromIl2Cpp]
    public List<GameObject> Barricades { get; set; } = [];

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        Clear();
    }

    public void Clear()
    {
        foreach (var barricade in Barricades)
        {
            Destroy(barricade);
        }

        FutureBlocks.Clear();
        VentsBlocked.Clear();
        Barricades.Clear();
    }

    public void LobbyStart()
    {
        Clear();
    }

    public void SetupBarricades()
    {
        foreach (var ventId in FutureBlocks)
        {
            VentsBlocked.Add(ventId);

            GameObject barricade = new("Barricade");

            Vent? trueVent = Helpers.GetVentById(ventId);

            if (trueVent == null) continue;

            barricade.transform.SetParent(trueVent.transform);
            barricade.gameObject.layer = trueVent.gameObject.layer;

            SpriteRenderer render = barricade.AddComponent<SpriteRenderer>();

            switch (ShipStatus.Instance.Type)
            {
                case ShipStatus.MapType.Fungle:
                    render.sprite = TouAssets.BarricadeFungleSprite.LoadAsset();
                    barricade.transform.localPosition = new Vector3(0.03f, -0.107f, -0.001f);
                    break;
                case ShipStatus.MapType.Pb:
                    render.sprite = TouAssets.BarricadeVentSprite.LoadAsset();
                    barricade.transform.localPosition = new Vector3(0, 0.05f, -0.001f);
                    barricade.transform.localScale = new Vector3(0.8f, 0.7f, 1f);
                    break;
                default:
                    render.sprite = TouAssets.BarricadeVentSprite.LoadAsset();
                    barricade.transform.localPosition = new Vector3(0, 0, -0.001f);
                    break;
            }
            if (trueVent.gameObject.name == "LowerCentralVent" && ModCompatibility.IsSubmerged())
            {
                barricade.transform.localPosition = new Vector3(0, 0.7f, -0.001f);
                barricade.transform.localScale = new Vector3(1.05f, 1.15f, 1.0625f);
            }
            if (ModCompatibility.IsLevelImpostor())
            {
                switch (ModCompatibility.GetLIVentType(trueVent))
                {
                    case "util-vent3":
                        render.sprite = TouAssets.BarricadeFungleSprite.LoadAsset();
                        barricade.transform.localPosition = new Vector3(0.03f, -0.107f, -0.001f);
                        break;
                    case "util-vent2":
                        render.sprite = TouAssets.BarricadeVentSprite.LoadAsset();
                        barricade.transform.localPosition = new Vector3(0, 0.05f, -0.001f);
                        barricade.transform.localScale = new Vector3(0.8f, 0.7f, 1f);
                        break;
                    default:
                        render.sprite = TouAssets.BarricadeVentSprite.LoadAsset();
                        barricade.transform.localPosition = new Vector3(0, 0, -0.001f);
                        break;
                }
            }

            Barricades.Add(barricade);
        }
        FutureBlocks.Clear();
    }

    public static IEnumerator SeeVenter(PlayerControl plumber)
    {
        var playersInVent = PlayerControl.AllPlayerControls.ToArray().Where(x => x.inVent);

        foreach (var player in playersInVent)
        {
            player.AddModifier<PlumberVenterModifier>(plumber, Color.white);
        }

        yield return new WaitForSeconds(1f);

        foreach (var player in ModifierUtils.GetPlayersWithModifier<PlumberVenterModifier>(x => x.Owner == plumber))
        {
            player.RemoveModifier<PlumberVenterModifier>();
        }
    }

    [MethodRpc((uint)TownOfUsRpc.PlumberFlush, SendImmediately = true)]
    public static void RpcPlumberFlush(PlayerControl player)
    {
        if (player.Data.Role is not PlumberRole)
        {
            Logger<TownOfUsPlugin>.Error("RpcPlumberFlush - Invalid Plumber");
            return;
        }
        var touAbilityEvent = new TouAbilityEvent(AbilityType.PlumberFlush, player);
        MiraEventManager.InvokeEvent(touAbilityEvent);

        if (PlayerControl.LocalPlayer.inVent)
        {
            PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(Vent.currentVent.Id);
            PlayerControl.LocalPlayer.MyPhysics.ExitAllVents();

            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Plumber));
        }

        if (!player.AmOwner) return;
        var someoneInVent = PlayerControl.AllPlayerControls.ToArray().Any(x => x.inVent);
        if (!someoneInVent) return;

        Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Plumber));
        Coroutines.Start(SeeVenter(player));
    }

    [MethodRpc((uint)TownOfUsRpc.PlumberBlockVent, SendImmediately = true)]
    public static void RpcPlumberBlockVent(PlayerControl player, int ventId)
    {
        if (player.Data.Role is not PlumberRole plumber)
        {
            Logger<TownOfUsPlugin>.Error("RpcPlumberBlockVent - Invalid Plumber");
            return;
        }

        if (!plumber.FutureBlocks.Contains(ventId))
            plumber.FutureBlocks.Add(ventId);
        
        var touAbilityEvent = new TouAbilityEvent(AbilityType.PlumberBlock, player, Helpers.GetVentById(ventId));
        MiraEventManager.InvokeEvent(touAbilityEvent);
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }
    public string GetAdvancedDescription()
    {
        return "水管工是一名船员支援型角色，可以在通风口上设置路障，并将所有潜伏者从通风口中冲出来。"
               + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("冲洗",
            $"冲洗通风口会让所有通风口开关一次，将所有正在通风口中的玩家踢出。水管工还会获得指向每个被冲出的玩家的箭头，持续一秒。",
            TouCrewAssets.FlushSprite),
        new("路障",
            $"在选定的通风口设置路障，下回合阻止玩家使用该通风口。",
            TouCrewAssets.BarricadeSprite),
    ];
}
