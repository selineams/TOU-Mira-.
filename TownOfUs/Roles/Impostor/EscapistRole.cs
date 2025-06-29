using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TownOfUs.Events.TouEvents;
using TownOfUs.Modules.Anims;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options;
using TownOfUs.Options.Roles.Impostor;
using TownOfUs.Patches.Stubs;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Impostor;

public sealed class EscapistRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    public string RoleName => "逃逸者";
    public string RoleDescription => "轻松脱离击杀现场";
    public string RoleLongDescription => "传送离开犯罪现场";
    public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<TransporterRole>());
    public Color RoleColor => TownOfUsColors.Impostor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public DoomableType DoomHintType => DoomableType.Protective;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Escapist,
        IntroSound = TouAudio.TimeLordIntroSound,
        CanUseVent = OptionGroupSingleton<EscapistOptions>.Instance.CanVent,
    };

    public Vector2? MarkedLocation { get; set; }
    public GameObject? EscapeMark { get; set; }
    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);
        EscapeMark?.gameObject.Destroy();
    }

    [MethodRpc((uint)TownOfUsRpc.Recall, SendImmediately = true)]
    public static void RpcRecall(PlayerControl player)
    {
        if (player.Data.Role is not EscapistRole)
        {
            Logger<TownOfUsPlugin>.Error("RpcRecall - Invalid escapist");
            return;
        }
        var touAbilityEvent = new TouAbilityEvent(AbilityType.EscapistRecall, player);
        MiraEventManager.InvokeEvent(touAbilityEvent);
    }

    [MethodRpc((uint)TownOfUsRpc.MarkLocation, SendImmediately = true)]
    public static void RpcMarkLocation(PlayerControl player, Vector2 pos)
    {
        if (player.Data.Role is not EscapistRole henry)
        {
            Logger<TownOfUsPlugin>.Error("RpcRecall - Invalid escapist");
            return;
        }

        var touAbilityEvent = new TouAbilityEvent(AbilityType.EscapistMark, player);
        MiraEventManager.InvokeEvent(touAbilityEvent);

        henry.MarkedLocation = pos;
        henry.EscapeMark = AnimStore.SpawnAnimAtPlayer(player, TouAssets.EscapistMarkPrefab.LoadAsset());
        henry.EscapeMark.transform.localPosition = new Vector3(pos.x, pos.y + 0.3f, 0.1f);
        henry.EscapeMark.SetActive(false);
    }
    public void FixedUpdate()
    {
        if (Player == null || Player.Data.Role is not EscapistRole || Player.HasDied()) return;
        if (EscapeMark != null)
        {
            EscapeMark.SetActive(PlayerControl.LocalPlayer.IsImpostor() || (PlayerControl.LocalPlayer.HasDied() && OptionGroupSingleton<GeneralOptions>.Instance.TheDeadKnow));
            if (MarkedLocation == null)
            {
                EscapeMark.gameObject.Destroy();
                EscapeMark = null;
            }
        }
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return "逃逸者是一名伪装者隐蔽型角色，可以标记一个位置并随后传送（回溯）到该位置。"
            + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("标记",
            "标记一个位置以备后用。",
            TouImpAssets.MarkSprite),
        new("回溯",
            "回溯到已标记的位置。",
            TouImpAssets.RecallSprite)
    ];
}
