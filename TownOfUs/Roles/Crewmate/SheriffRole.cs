using System.Globalization;
using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class SheriffRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITouCrewRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "警长";
    public string RoleDescription => "射杀<color=#FF0000FF>伪装者</color>";
    public string RoleLongDescription => "击杀伪装者，但不要误杀船员";
    public Color RoleColor => TownOfUsColors.Sheriff;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateKilling;
    public DoomableType DoomHintType => DoomableType.Relentless;
    public bool IsPowerCrew => true; // Always disable end game checks with a sheriff around
    public override bool IsAffectedByComms => false;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Sheriff,
        IntroSound = CustomRoleUtils.GetIntroSound(RoleTypes.Impostor),
    };

    public static void OnRoundStart()
    {
        CustomButtonSingleton<SheriffShootButton>.Instance.Usable = true;
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);
        var missType = OptionGroupSingleton<SheriffOptions>.Instance.MisfireType;
        
        if (CustomButtonSingleton<SheriffShootButton>.Instance.FailedShot)
        {
            stringB.AppendLine(CultureInfo.InvariantCulture, $"<b>你已无法再开枪。</b>");
        }
        else switch (missType)
            {
                case MisfireOptions.Both:
                    stringB.AppendLine(CultureInfo.InvariantCulture, $"<b>误伤会导致你和目标同归于尽。</b>");
                    break;
                case MisfireOptions.Sheriff:
                    stringB.AppendLine(CultureInfo.InvariantCulture, $"<b>误伤会导致自杀。</b>");
                    break;
                case MisfireOptions.Target:
                    stringB.AppendLine(CultureInfo.InvariantCulture, $"<b>误伤会导致目标死亡，且你失去开枪能力。</b>");
                    break;
                default:
                    stringB.AppendLine(CultureInfo.InvariantCulture, $"<b>误伤后你将无法再开枪。</b>");
                    break;
            }
        if (PlayerControl.LocalPlayer.TryGetModifier<AllianceGameModifier>(out var allyMod) && !allyMod.GetsPunished)
        {
            stringB.AppendLine(CultureInfo.InvariantCulture, $"<b>你可以无惩罚地开枪。</b>");
        }

        return stringB;
    }
    public string GetAdvancedDescription()
    {
        return $"警长是一名船员击杀型角色，可以射击玩家尝试击杀他们。如果警长因误伤未死亡，将失去开枪能力。" + MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("射击",
            $"射击一名玩家尝试击杀他们，如果目标不是伪装者或其他可射击阵营则会误伤。",
            TouCrewAssets.SheriffShootSprite)
    ];
}
