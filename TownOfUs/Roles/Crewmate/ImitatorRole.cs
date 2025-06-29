using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules.Wiki;
using TownOfUs.Patches.Stubs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Roles.Crewmate;

public sealed class ImitatorRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "效颦者";
    public string RoleDescription => "利用死亡角色帮助船员";
    public string RoleLongDescription => "借助忠诚船员的亡魂再次帮助船员";
    public Color RoleColor => TownOfUsColors.Imitator;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateSupport;
    public DoomableType DoomHintType => DoomableType.Perception;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Imitator,
        IntroSound = TouAudio.SpyIntroSound,
    };
    public override void Initialize(PlayerControl player)
    {
        RoleStubs.RoleBehaviourInitialize(this, player);
        player.AddModifier<ImitatorCacheModifier>();
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription()
    {
        return "效颦者是一名船员支援型角色，可以选择一名死亡船员模仿其角色。" +
            "你会获得该角色及其技能，直到你更换目标。 " +
            "如果有多名效颦者存活，部分角色将无法被模仿。"
            + MiscUtils.AppendOptionsText(GetType());
    }
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } = [
        new("模仿船员",
            $"除效颦者和船员外，所有船员角色均可被模仿。政治家、市长、检察官和典狱长,"
            + "仅在没有其他效颦者时可选，典狱长和检察官无法使用会议技能，侠客没有安全击杀。",
            TouCrewAssets.InspectSprite),
        new("中立对应表",
            "失忆者⇨灵媒 | "
            + "末日预言者⇨侠客 | "
            + "行刑者⇨告密者\n"
            + "混沌⇨警长 | "
            + " 守护天使⇨牧师 | "
            + "审判官⇨神谕者\n"
            + "小丑⇨换票师 | "
            + " 雇佣兵⇨护卫者\n"
            + "瘟疫之源/万疫之神⇨灵气探 | "
            + "噬魂兽⇨招魂师 | "
            + "月下狼人⇨巡猎者",
            TouNeutAssets.GuardSprite),
        new("伪装者对应",
            "爆破手⇨陷阱师 | "
            + "逃逸者⇨传送师\n"
            + "催眠师⇨观测者 | "
            + "清理者⇨侧写师\n"
            + "管道工⇨工程师 | "
            + "赏金猎人⇨追踪者\n"
            + "送葬者⇨殉道者 | "
            + "巫术师⇨老兵",
            TouImpAssets.DragSprite),
    ];
    public string SecondTabName => "角色指南";
}
