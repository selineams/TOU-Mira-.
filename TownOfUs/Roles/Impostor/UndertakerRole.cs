using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Events.TouEvents;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Impostor;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Impostor;

public sealed class UndertakerRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    public string RoleName => "送葬者";
    public string RoleDescription => "拖动尸体并隐藏";
    public string RoleLongDescription => "拖动尸体到地图各处以隐藏，防止被举报";
    public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<AltruistRole>());
    public Color RoleColor => TownOfUsColors.Impostor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorSupport;
    public DoomableType DoomHintType => DoomableType.Death;
    public CustomRoleConfiguration Configuration => new(this)
    {
        UseVanillaKillButton = true,
        CanUseVent = OptionGroupSingleton<UndertakerOptions>.Instance.CanVent,
        Icon = TouRoleIcons.Undertaker,
    };
    public void FixedUpdate()
    {
        if (Player == null || Player.Data.Role is not JanitorRole || Player.HasDied() || !Player.AmOwner || MeetingHud.Instance || (!HudManager.Instance.UseButton.isActiveAndEnabled && !HudManager.Instance.PetButton.isActiveAndEnabled)) return;
        HudManager.Instance.KillButton.ToggleVisible(OptionGroupSingleton<UndertakerOptions>.Instance.UndertakerKill || (Player != null && Player.GetModifiers<BaseModifier>().Any(x => x is ICachedRole)) || (Player != null && MiscUtils.ImpAliveCount == 1));
    }

    [MethodRpc((uint)TownOfUsRpc.DragBody, LocalHandling = RpcLocalHandling.Before, SendImmediately = true)]
    public static void RpcStartDragging(PlayerControl playerControl, byte bodyId)
    {
        playerControl.GetModifierComponent()?.AddModifier(new DragModifier(bodyId));

        var touAbilityEvent = new TouAbilityEvent(AbilityType.UndertakerDrag, playerControl, Helpers.GetBodyById(bodyId));
        MiraEventManager.InvokeEvent(touAbilityEvent);

        if (playerControl.AmOwner)
        {
            CustomButtonSingleton<UndertakerDragDropButton>.Instance.SetDrop();
        }
    }

    [MethodRpc((uint)TownOfUsRpc.DropBody, LocalHandling = RpcLocalHandling.Before, SendImmediately = true)]
    public static void RpcStopDragging(PlayerControl playerControl, Vector2 dropLocation)
    {
        var dragMod = playerControl.GetModifier<DragModifier>()!;
        var dropPos = (Vector3)dropLocation;
        dropPos.z = dropPos.y / 1000f;
        dragMod.DeadBody!.transform.position = dropPos;

        var touAbilityEvent = new TouAbilityEvent(AbilityType.UndertakerDrop, playerControl, dragMod.DeadBody);
        MiraEventManager.InvokeEvent(touAbilityEvent);

        playerControl.GetModifierComponent()?.RemoveModifier(dragMod);

        if (playerControl.AmOwner)
        {
            CustomButtonSingleton<UndertakerDragDropButton>.Instance.SetDrag();
        }
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return "送葬者是一名伪装者辅助型角色，可以拖动尸体到地图各处。" + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("拖拽",
            "拖动尸体，如设置允许可拖入管道。",
            TouImpAssets.DragSprite),
        new("放下",
            "放下被拖拽的尸体，停止继续拖动。",
            TouImpAssets.DropSprite)      
    ];
}
