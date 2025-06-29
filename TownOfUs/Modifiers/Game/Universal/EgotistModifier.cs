using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Alliance;

public sealed class EgotistModifier : AllianceGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "营己徒";
    public override string IntroInfo => $"你的自我正在膨胀...";
    public override string Symbol => "#";
    public override float IntroSize => 4f;
    public override bool DoesTasks => false;
    public override bool GetsPunished => false;
    public override bool CrewContinuesGame => false;
    public override ModifierFaction FactionType => ModifierFaction.CrewmateAlliance;
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Egotist;
    public override string GetDescription() => "<size=130%>背叛船员，</size>\n<size=125%>与杀手一同获胜。</size>";
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<AllianceModifierOptions>.Instance.EgotistChance;
    public override int GetAmountPerGame() => 1;
    
    public int Priority { get; set; } = 5;
    public List<CustomButtonWikiDescription> Abilities { get; } = [];

    public static bool EgoVisibilityFlag(PlayerControl player)
    {
        return player.HasModifier<EgotistModifier>() && (PlayerControl.LocalPlayer.IsImpostor() || player.Is(RoleAlignment.NeutralKilling));
    }

    public string GetAdvancedDescription()
    {
        return $"营己徒是船员联盟修饰（以<color=#669966>#</color>为标识）。作为营己徒，只有船员失败你才能获胜，即使你已死亡。如果会议结束后没有船员存活，你会直接胜利离场，但游戏会继续。";
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate();
    }
    public override bool? DidWin(GameOverReason reason)
    {
        return !(reason is GameOverReason.CrewmatesByVote or GameOverReason.CrewmatesByTask or GameOverReason.ImpostorDisconnect);
    }
}
