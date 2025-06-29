using MiraAPI.Roles;
using UnityEngine;

namespace TownOfUs.Roles;

public static class TouRoleGroups
{
    public static RoleOptionsGroup CrewInvest { get; } = new("船员调查型", TownOfUsColors.Crewmate);
    public static RoleOptionsGroup CrewKiller { get; } = new("船员击杀型", TownOfUsColors.Crewmate);
    public static RoleOptionsGroup CrewProc { get; } = new("船员保护型", TownOfUsColors.Crewmate);
    public static RoleOptionsGroup CrewPower { get; } = new("船员强力型", TownOfUsColors.Crewmate);
    public static RoleOptionsGroup CrewSup { get; } = new("船员辅助型", TownOfUsColors.Crewmate);
    public static RoleOptionsGroup NeutralBenign { get; } = new("中立善良型", Color.gray);
    public static RoleOptionsGroup NeutralEvil { get; } = new("中立邪恶型", Color.gray);
    public static RoleOptionsGroup NeutralKiller { get; } = new("中立击杀型", Color.gray);
    public static RoleOptionsGroup ImpConceal { get; } = new("内鬼隐藏型", TownOfUsColors.ImpSoft);
    public static RoleOptionsGroup ImpKiller { get; } = new("内鬼击杀型", TownOfUsColors.ImpSoft);
    public static RoleOptionsGroup ImpSup { get; } = new("内鬼辅助型", TownOfUsColors.ImpSoft);
}
