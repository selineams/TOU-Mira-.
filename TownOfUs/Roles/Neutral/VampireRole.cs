using System.Globalization;
using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs.Events.TouEvents;
using TownOfUs.Modifiers.Game.Neutral;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Neutral;

public sealed class VampireRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "吸血魔";
    public string RoleDescription => "转化船员并杀死其他人";
    public string RoleLongDescription => "咬所有其他玩家";
    public Color RoleColor => TownOfUsColors.Vampire;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    public DoomableType DoomHintType => DoomableType.Death;
    public CustomRoleConfiguration Configuration => new(this)
    {
        CanUseVent = OptionGroupSingleton<VampireOptions>.Instance.CanVent,
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Phantom),
        Icon = TouRoleIcons.Vampire,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>(),
        MaxRoleCount = 1,
    };
    public bool HasImpostorVision => OptionGroupSingleton<VampireOptions>.Instance.HasVision;
    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);
        if (Player.AmOwner)
        {
            HudManager.Instance.ImpostorVentButton.graphic.sprite = TouNeutAssets.VampVentSprite.LoadAsset();
            HudManager.Instance.ImpostorVentButton.buttonLabelText.SetOutlineColor(TownOfUsColors.Vampire);
        }
    }
    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);
        if (Player.AmOwner)
        {
            HudManager.Instance.ImpostorVentButton.graphic.sprite = TouAssets.VentSprite.LoadAsset();
            HudManager.Instance.ImpostorVentButton.buttonLabelText.SetOutlineColor(TownOfUsColors.Impostor);
        }
    }

    //public bool CanChangeRole => false;
    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }

        Console console = usable.TryCast<Console>()!;
        return (console == null) || console.AllowImpostor;
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return WinConditionMet();
    }

    public bool WinConditionMet()
    {
        var vampireCount = CustomRoleUtils.GetActiveRolesOfType<VampireRole>().Count(x => !x.Player.HasDied());

        if (MiscUtils.KillersAliveCount > vampireCount) return false;

        return vampireCount >= Helpers.GetAlivePlayers().Count - vampireCount;
    }

    [MethodRpc((uint)TownOfUsRpc.VampireBite, SendImmediately = true)]
    public static void RpcVampireBite(PlayerControl player, PlayerControl target)
    {
        if (player.Data.Role is not VampireRole)
        {
            Logger<TownOfUsPlugin>.Error("RpcVampireBite - Invalid vampire");
            return;
        }

        var touAbilityEvent = new TouAbilityEvent(AbilityType.VampireBite, player, target);
        MiraEventManager.InvokeEvent(touAbilityEvent);

        target.ChangeRole(RoleId.Get<VampireRole>());
        target.AddModifier<VampireBittenModifier>();
        if (OptionGroupSingleton<VampireOptions>.Instance.CanGuessAsNewVamp)
        {
            target.AddModifier<NeutralKillerAssassinModifier>();
        }

        // if (source.AmOwner && target.TryGetModifier<LoverModifier>(out var lovers) && lovers.OtherLover!.Data.Role is not VampireRole)
        //{
        //    MiscUtils.ChangeRole(lovers.OtherLover, RoleType.Vampire);
        //    lovers.OtherLover.GetModifierComponent()?.AddModifier<BittenModifier>();
        //}
    }
    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var alignment = RoleAlignment.ToDisplayString().Replace("中立", "<color=#8A8A8AFF>中立");


        var stringB = new StringBuilder();
        stringB.AppendLine(CultureInfo.InvariantCulture, $"{RoleColor.ToTextColor()}你是<b> {RoleName}。</b></color>");
        stringB.AppendLine(CultureInfo.InvariantCulture, $"<size=60%>阵营: <b>{alignment}</color></b></size>");
        stringB.Append("<size=70%>");
        stringB.AppendLine(CultureInfo.InvariantCulture, $"{RoleLongDescription}");

        return stringB;
    }

    public string GetAdvancedDescription()
    {
        return "吸血魔是一名中立击杀型角色，通过成为场上最后的杀手获胜。可以咬人将其转化为吸血魔，或直接击杀玩家。" + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("咬击",
            "咬一名玩家。如果被咬者是船员且未超过最大吸血魔数量，则会转化为吸血魔，否则直接击杀。",
            TouNeutAssets.BiteSprite)
    ];
}
