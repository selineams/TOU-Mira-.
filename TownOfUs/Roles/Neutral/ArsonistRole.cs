using System.Globalization;
using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs.Buttons.Neutral;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Neutral;

public sealed class ArsonistRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "纵火狂";
    public string RoleDescription => "浇油点火，焚尽一切";
    public string RoleLongDescription => OptionGroupSingleton<ArsonistOptions>.Instance.LegacyArsonist ? "给玩家浇油并点燃最近的目标，焚烧所有被浇油的玩家" : "给玩家浇油并点燃，焚烧周围所有被浇油的玩家";
    public Color RoleColor => TownOfUsColors.Arsonist;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    public DoomableType DoomHintType => DoomableType.Fearmonger;
    public CustomRoleConfiguration Configuration => new(this)
    {
        CanUseVent = OptionGroupSingleton<ArsonistOptions>.Instance.CanVent,
        IntroSound = TouAudio.ArsoIgniteSound,
        MaxRoleCount = 1,
        Icon = TouRoleIcons.Arsonist,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>(),
    };
    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);
        if (Player.AmOwner)
        {
            HudManager.Instance.ImpostorVentButton.graphic.sprite = TouNeutAssets.ArsoVentSprite.LoadAsset();
            HudManager.Instance.ImpostorVentButton.buttonLabelText.SetOutlineColor(TownOfUsColors.Arsonist);
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

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);

        var allDoused = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.HasDied() && x.GetModifier<ArsonistDousedModifier>()?.ArsonistId == Player.PlayerId);

        if (allDoused.Any())
        {
            stringB.Append("\n<b>已浇油玩家:</b>");
            foreach (var plr in allDoused)
            {
                stringB.Append(CultureInfo.InvariantCulture, $"\n{Color.white.ToTextColor()}{plr.Data.PlayerName}</color>");
            }
        }

        return stringB;
    }

    public override void OnDeath(DeathReason reason)
    {
        var button = CustomButtonSingleton<ArsonistIgniteButton>.Instance;

        if (button != null && button.Ignite != null)
        {
            button.Ignite.Clear();
            button.Ignite = null;
        }

        RoleStubs.RoleBehaviourOnDeath(this, reason);
    }

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
        if (Player.HasDied()) return false;

        var result = Helpers.GetAlivePlayers().Count <= 2 && MiscUtils.KillersAliveCount == 1;

        return result;
    }

    public string GetAdvancedDescription()
    {
        return "纵火狂是一名中立击杀型角色，通过成为场上最后一名杀手获胜。" + (OptionGroupSingleton<ArsonistOptions>.Instance.LegacyArsonist ? "可以给玩家浇油并点燃其中一人，点燃后地图上所有被浇油的玩家都会被焚烧。" : "可以给玩家浇油并在靠近时点燃，焚烧周围所有被浇油的玩家。") + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("浇油",
            "给一名玩家浇汽油",
            TouNeutAssets.DouseButtonSprite),
        new("点燃",
            OptionGroupSingleton<ArsonistOptions>.Instance.LegacyArsonist ? "只要点燃一名附近玩家，即可焚烧全图所有被浇油的玩家。" : "点燃你周围范围内所有被浇油的玩家。",
            TouNeutAssets.IgniteButtonSprite)
    ];
}
