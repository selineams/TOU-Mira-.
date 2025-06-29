using System.Globalization;
using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Impostor;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Impostor;

public sealed class MorphlingRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "化形者";
    public string RoleDescription => "变身为船员";
    public string RoleLongDescription => "采样玩家并变身为其伪装自己。每回合开始时采样会被清除。";
    public Color RoleColor => TownOfUsColors.Impostor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public DoomableType DoomHintType => DoomableType.Perception;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Morphling,
        CanUseVent = OptionGroupSingleton<MorphlingOptions>.Instance.MorphlingVent,
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Shapeshifter),
    };

    public PlayerControl? Sampled { get; set; }

    public override void OnVotingComplete()
    {
        RoleStubs.RoleBehaviourOnVotingComplete(this);

        Clear();
    }

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);
        CustomButtonSingleton<MorphlingMorphButton>.Instance.SetActive(false, this);
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        Clear();
    }

    public void Clear()
    {
        Sampled = null;
    }

    public void LobbyStart()
    {
        Clear();
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);

        if (Player.HasModifier<MorphlingMorphModifier>())
        {
            stringB.Append(CultureInfo.InvariantCulture, $"\n<b>当前变身：</b> {Sampled!.Data.Color.ToTextColor()}{Sampled.Data.PlayerName}</color>");
        }

        return stringB;
    }

    public string GetAdvancedDescription()
    {
        return "化形者是一名伪装者隐蔽型角色，可以采样一名玩家并变身为其外观。"
            + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("采样",
            "采集一名玩家的DNA，之后可变身为其。",
            TouImpAssets.SampleSprite),
        new("变身",
            "变身为已采样玩家的外观，可提前取消。",
            TouImpAssets.MorphSprite)
    ];
}
