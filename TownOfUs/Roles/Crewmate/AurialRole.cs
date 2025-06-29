using System.Text;
using System.Collections;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;
using Color = UnityEngine.Color;
using Object = UnityEngine.Object;

namespace TownOfUs.Roles.Crewmate;

public sealed class AurialRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "灵气探";
    public string RoleDescription => "感知你灵气中的扰动。";
    public string RoleLongDescription => "你能感知灵气范围内任何玩家使用技能的行为。";
    public Color RoleColor => TownOfUsColors.Aurial;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;
    public DoomableType DoomHintType => DoomableType.Perception;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Aurial,
        IntroSound = TouAudio.MediumIntroSound,
    };

    private readonly Dictionary<(Vector3, int), ArrowBehaviour> _senseArrows = new Dictionary<(Vector3, int), ArrowBehaviour>();

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        _senseArrows.Values.DestroyAll();
        _senseArrows.Clear();
    }

    public void LobbyStart()
    {
        _senseArrows.Values.DestroyAll();
        _senseArrows.Clear();
    }

    [HideFromIl2Cpp]
    public IEnumerator Sense(PlayerControl player)
    {
        if (!CheckRange(player, OptionGroupSingleton<AurialOptions>.Instance.AuraOuterRadius)) yield break;

        var position = player.transform.position;
        var colorID = player.Data.DefaultOutfit.ColorId;
        var color = Color.white;

        if (CheckRange(player, OptionGroupSingleton<AurialOptions>.Instance.AuraInnerRadius)/* && !CamouflageUnCamouflage.IsCamoed*/)
            color = Palette.PlayerColors[colorID];

        var arrow = MiscUtils.CreateArrow(Player.transform, color);
        arrow.target = position;

        try { DestroyArrow(position, colorID); }
        catch { /* ignored */ }

        _senseArrows.Add((position, colorID), arrow);

        yield return (object)new WaitForSeconds(OptionGroupSingleton<AurialOptions>.Instance.SenseDuration);

        try { DestroyArrow(position, colorID); }
        catch { /* ignored */ }
    }

    public bool CheckRange(PlayerControl player, float radius)
    {
        float lightRadius = radius * ShipStatus.Instance.MaxLightRadius;
        Vector2 vector2 = new Vector2(player.GetTruePosition().x - Player.GetTruePosition().x, player.GetTruePosition().y - Player.GetTruePosition().y);
        float magnitude = vector2.magnitude;

        if (magnitude <= lightRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DestroyArrow(Vector3 targetArea, int colourID)
    {
        var arrow = _senseArrows.FirstOrDefault(x => x.Key == (targetArea, colourID));

        if (arrow.Value != null)
            Object.Destroy(arrow.Value);

        if (arrow.Value?.gameObject != null)
            Object.Destroy(arrow.Value.gameObject);

        _senseArrows.Remove(arrow.Key);
    }

    [MethodRpc((uint)TownOfUsRpc.AurialSense, SendImmediately = true)]
    public static void RpcSense(PlayerControl player, PlayerControl source)
    {
        if (player.Data.Role is not AurialRole aurial)
        {
            Logger<TownOfUsPlugin>.Error("Invalid Aurial");
            return;
        }

        if (player.AmOwner)
        {
            Coroutines.Start(aurial.Sense(source));
        }
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return "灵气探是一名船员调查型角色，附近有玩家使用技能时会收到提示。" + MiscUtils.AppendOptionsText(GetType());
    }
}
