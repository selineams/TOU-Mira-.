using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfUs.Modifiers;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Neutral;

public sealed class SurvivorRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "幸存者";
    public string RoleDescription => "不择手段活下去";
    public string RoleLongDescription => "活到最后即可与任意阵营一同获胜";
    public Color RoleColor => TownOfUsColors.Survivor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralBenign;
    public DoomableType DoomHintType => DoomableType.Protective;
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.ToppatIntroSound,
        Icon = TouRoleIcons.Survivor,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>(),
    };

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("防弹衣",
            "穿上防弹衣保护自己免受攻击。",
            TouNeutAssets.VestSprite)
    ];

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return "幸存者是一名中立善良型角色，只需要活到游戏结束即可获胜。" + MiscUtils.AppendOptionsText(GetType());
    }

    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);

        if (Player.AmOwner && OptionGroupSingleton<SurvivorOptions>.Instance.ScatterOn)
        {
            Player.AddModifier<ScatterModifier>(OptionGroupSingleton<SurvivorOptions>.Instance.ScatterTimer);
        }
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleStubs.RoleBehaviourDeinitialize(this, targetPlayer);

        if (Player.AmOwner && OptionGroupSingleton<SurvivorOptions>.Instance.ScatterOn)
        {
            Player.RemoveModifier<ScatterModifier>();
        }
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return !Player.HasDied();
    }
}
