using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class CelebrityModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "名人";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Celebrity;
    public override string GetDescription() => "你死亡时会公布你的死因。";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePostmortem;
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.CelebrityChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.CelebrityAmount != 0 ? 1 : 0;
    public List<CustomButtonWikiDescription> Abilities { get; } = [];
    
    public DateTime DeathTime { get; set; }
    public float DeathTimeMilliseconds { get; set; }
    public string DeathMessage { get; set; }
    public string AnnounceMessage { get; set; }
    public string StoredRoom { get; set; }
    public bool Announced { get; set; }

    public string GetAdvancedDescription()
    {
        return "你死后，你的死亡细节会被公布，包括你被杀的位置和杀你的角色，并会在会议中显示。";
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate();
    }

    public static void CelebrityKilled(PlayerControl source, PlayerControl player, string customDeath = "")
    {
        if (!player.HasModifier<CelebrityModifier>())
        {
            Logger<TownOfUsPlugin>.Error("RpcCelebrityKilled - Invalid Celebrity");
            return;
        }

        PlainShipRoom? plainShipRoom = null;

        var allRooms2 = ShipStatus.Instance.FastRooms;
        foreach (PlainShipRoom plainShipRoom2 in allRooms2.Values)
        {
            if (plainShipRoom2.roomArea && plainShipRoom2.roomArea.OverlapPoint(player.GetTruePosition()))
            {
                plainShipRoom = plainShipRoom2;
            }
        }

        var room = plainShipRoom != null ? TranslationController.Instance.GetString(plainShipRoom.RoomId) : "室外/走廊";

        var celeb = player.GetModifier<CelebrityModifier>()!;
        celeb.StoredRoom = room;
        celeb.DeathTime = DateTime.UtcNow;

        celeb.AnnounceMessage = $"<size=90%>名人 {player.GetDefaultAppearance().PlayerName} 已死亡!</size>\n<size=70%>(详情见聊天框)</size>";

        var cod = "被杀";
        switch (source.Data.Role)
        {
            case SheriffRole or HunterRole or VeteranRole:
                cod = "正义执行";
                break;
            case InquisitorRole:
                cod = "审判";
                break;
            case ArsonistRole:
                cod = "烧";
                break;
            case GlitchRole:
                cod = "混沌";
                break;
            case JuggernautRole:
                cod = "天启创";
                break;
            case PestilenceRole:
                cod = "疫病";
                break;
            case SoulCollectorRole:
                cod = "收割";
                break;
            case VampireRole:
                cod = "咬";
                break;
            case WerewolfRole:
                cod = "月狼抓";
                break;
        }
        if (customDeath != string.Empty && customDeath != "") cod = customDeath;
        if (MeetingHud.Instance)
        {
            celeb.Announced = true;
        }

        if (source == player)
            celeb.DeathMessage = $"名人 {player.GetDefaultAppearance().PlayerName} 死亡! 地点: {celeb.StoredRoom}, 死因: 自杀, 时间: ";
        else
            celeb.DeathMessage = $"名人 {player.GetDefaultAppearance().PlayerName} 遭{cod}! 地点: {celeb.StoredRoom}, 凶手: {source.Data.Role.NiceName}, 时间: ";
    }

    [MethodRpc((uint)TownOfUsRpc.UpdateCelebrityKilled, SendImmediately = true)]
    public static void RpcUpdateCelebrityKilled(PlayerControl player, float milliseconds)
    {
        if (!player.HasModifier<CelebrityModifier>())
        {
            Logger<TownOfUsPlugin>.Error("RpcUpdateCelebrityKilled - Invalid Celebrity");
            return;
        }

        Logger<TownOfUsPlugin>.Error($"RpcUpdateCelebrityKilled milliseconds: {milliseconds}");

        var celeb = player.GetModifier<CelebrityModifier>()!;

        celeb.DeathTimeMilliseconds = milliseconds;
    }
}
