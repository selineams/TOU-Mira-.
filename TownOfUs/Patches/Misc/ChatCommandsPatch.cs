using System.Globalization;
using HarmonyLib;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TownOfUs.Modules;
using TownOfUs.Options;
using TownOfUs.Patches.Options;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Patches.Misc;

[HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
public static class ChatPatches
{
    // ReSharper disable once InconsistentNaming
    public static bool Prefix(ChatController __instance)
    {
        var text = __instance.freeChatField.Text.ToLower(CultureInfo.InvariantCulture);
        var textRegular = __instance.freeChatField.Text;

        if (text.Replace(" ", string.Empty).StartsWith("/", StringComparison.OrdinalIgnoreCase)
            && text.Replace(" ", string.Empty).Contains("summary", StringComparison.OrdinalIgnoreCase))
        {
            var title = $"<color=#8BFDFD>系统</color>";
            var msg = "没有可显示的游戏总结！";
            if (GameHistory.EndGameSummary != string.Empty)
            {
                var factionText = string.Empty;
                if (GameHistory.WinningFaction != string.Empty) factionText = $"<size=80%>获胜阵营: {GameHistory.WinningFaction}</size>\n";
                title = $"<color=#8BFDFD>系统</color>\n<size=62%>{factionText}{GameHistory.EndGameSummary}</size>";
                msg = string.Empty;
            }
            MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, title, msg);

            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            __instance.quickChatField.Clear();
            __instance.UpdateChatMode();
            return false;
        }
        else if (text.Replace(" ", string.Empty).StartsWith("/nerfme", StringComparison.OrdinalIgnoreCase))
        {
            var title = $"<color=#8BFDFD>系统</color>";
            var msg = "你不能在大厅外削弱自己！";
            if (LobbyBehaviour.Instance)
            {
                VisionPatch.NerfMe = !VisionPatch.NerfMe;
                msg = $"切换削弱状态为 {VisionPatch.NerfMe}!";
            }
            MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, title, msg);
            
            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            __instance.quickChatField.Clear();
            __instance.UpdateChatMode();
            return false;
        }
        else if (text.Replace(" ", string.Empty).StartsWith("/setname", StringComparison.OrdinalIgnoreCase))
        {
            var title = $"<color=#8BFDFD>系统</color>";
                if (text.StartsWith("/setname ", StringComparison.OrdinalIgnoreCase))
                    textRegular = textRegular[9..];
                else if (text.StartsWith("/setname", StringComparison.OrdinalIgnoreCase))
                    textRegular = textRegular[8..];
                else if (text.StartsWith("/ setname ", StringComparison.OrdinalIgnoreCase))
                    textRegular = textRegular[10..];
                else if (text.StartsWith("/ setname", StringComparison.OrdinalIgnoreCase))
                    textRegular = textRegular[9..];
            var msg = "你不能在大厅外更改名字！";
            if (LobbyBehaviour.Instance)
            {
                if (textRegular.Length < 2)
                {
                    msg = $"玩家名至少需要2个字符！";
                }
                else
                {
                    // This is done to prevent the player from being kicked for changing their name as they're not the host
                    PlayerControl.LocalPlayer.CmdCheckName(textRegular);
                    msg = $"下局玩家名已更改为: {textRegular}";
                }
            }
            MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, title, msg);
            
            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            __instance.quickChatField.Clear();
            __instance.UpdateChatMode();
            return false;
        }
        else if (text.Replace(" ", string.Empty).StartsWith("/sethost", StringComparison.OrdinalIgnoreCase))
        {
            var title = $"<color=#8BFDFD>系统</color>";
                if (text.StartsWith("/sethost ", StringComparison.OrdinalIgnoreCase))
                    textRegular = textRegular[9..];
                else if (text.StartsWith("/sethost", StringComparison.OrdinalIgnoreCase))
                    textRegular = textRegular[8..];
                else if (text.StartsWith("/ sethost ", StringComparison.OrdinalIgnoreCase))
                    textRegular = textRegular[10..];
                else if (text.StartsWith("/ sethost", StringComparison.OrdinalIgnoreCase))
                    textRegular = textRegular[9..];
            var msg = "你不是当前房主！";
            if (PlayerControl.LocalPlayer.IsHost())
            {
                var playerCon = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => string.Equals(x.Data.PlayerName, textRegular, StringComparison.OrdinalIgnoreCase));
                var player = AmongUsClient.Instance.allClients.ToArray().FirstOrDefault(x => string.Equals(x.PlayerName, textRegular, StringComparison.OrdinalIgnoreCase));
                if (LobbyBehaviour.Instance && player != null && playerCon != null)
                {
                    msg = $"{textRegular} 现在是房主！\n" +
                    $"<size=75%>此命令为<b>实验性</b>。如果有玩家之后加入，必须由原房主再次运行此命令以恢复权限。新房主也无法更改服务器可见性。</size>";
                    RpcChangeHost(PlayerControl.LocalPlayer, player.Id, playerCon);
                }
                else if (LobbyBehaviour.Instance)
                {
                    msg = $"未找到指定玩家！({textRegular})";
                }
                else
                {
                    msg = "你不能在大厅外更改房主！";
                }
            }
            MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, title, msg);
            
            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            __instance.quickChatField.Clear();
            __instance.UpdateChatMode();
            return false;
        }
        else if (text.Replace(" ", string.Empty).StartsWith("/help", StringComparison.OrdinalIgnoreCase))
        {
            var title = $"<color=#8BFDFD>系统</color>";
            List<string> randomNames = ["Atony", "Alchlc", "angxlwtf", "Digi", "donners", "K3ndo", "MyDragonBreath", "Pietro", "twix", "xerm", "XtraCube", "Zeo", "Slushie"];
            var msg = "<size=75%>聊天命令:\n" +
                "/help - 显示本帮助信息\n" +
                $"/jail - 如果你是<b><color=#{Color.gray.ToHtmlStringRGBA()}>典狱长</color></b>，你可以通过输入<b>/jail 内容</b>给你的囚犯发送消息\n" +
                "/nerfme - 将你的视野减半\n" +
                $"/sethost - 更改房主为其他玩家，若有玩家之后加入需由原房主再次运行。\n" +
                $"/setname - 更改你的名字为命令后跟随的内容（如 /setname {randomNames.Random()}），下局生效。\n" +
                "/summary - 显示上一局游戏总结\n</size>";
            
            MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, title, msg);

            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            __instance.quickChatField.Clear();
            __instance.UpdateChatMode();
            return false;
        }
        else if (text.Replace(" ", string.Empty).StartsWith("/jail", StringComparison.OrdinalIgnoreCase))
        {
            var title = $"<color=#8BFDFD>系统</color>";
            
            MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, title, "该mod已不再支持/jail聊天，请使用游戏内红色聊天按钮。");

            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            __instance.quickChatField.Clear();
            __instance.UpdateChatMode();
            return false;
        }
        else if (text.Replace(" ", string.Empty).StartsWith("/", StringComparison.OrdinalIgnoreCase))
        {
            var title = $"<color=#8BFDFD>系统</color>";
            
            MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, title, "无效命令。如需聊天命令信息，请输入/help。如需了解职业或特性的作用，请点击右上角地球按钮查看游戏内百科。");

            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            __instance.quickChatField.Clear();
            __instance.UpdateChatMode();
            return false;
        }
        else if (TeamChatPatches.TeamChatActive && !PlayerControl.LocalPlayer.HasDied() && (PlayerControl.LocalPlayer.Data.Role is JailorRole || PlayerControl.LocalPlayer.IsJailed() ||PlayerControl.LocalPlayer.Data.Role is VampireRole || PlayerControl.LocalPlayer.IsImpostor()))
        {
            var genOpt = OptionGroupSingleton<GeneralOptions>.Instance;

            if (PlayerControl.LocalPlayer.Data.Role is JailorRole)
            {
                TeamChatPatches.RpcSendJailorChat(PlayerControl.LocalPlayer, textRegular);
                MiscUtils.AddTeamChat(PlayerControl.LocalPlayer.Data, $"<color=#{TownOfUsColors.Jailor.ToHtmlStringRGBA()}>{PlayerControl.LocalPlayer.Data.PlayerName} (狱警)</color>", textRegular, onLeft: false);

                __instance.freeChatField.Clear();
                __instance.quickChatMenu.Clear();
                __instance.quickChatField.Clear();
                __instance.UpdateChatMode();

                return false;
            }
            else if (PlayerControl.LocalPlayer.IsJailed())
            {
                TeamChatPatches.RpcSendJaileeChat(PlayerControl.LocalPlayer, textRegular);
                MiscUtils.AddTeamChat(PlayerControl.LocalPlayer.Data, $"<color=#{TownOfUsColors.Jailor.ToHtmlStringRGBA()}>{PlayerControl.LocalPlayer.Data.PlayerName} (被囚禁)</color>", textRegular, onLeft: false);

                __instance.freeChatField.Clear();
                __instance.quickChatMenu.Clear();
                __instance.quickChatField.Clear();
                __instance.UpdateChatMode();

                return false;
            }
            else if (PlayerControl.LocalPlayer.Data.Role is VampireRole && genOpt.VampireChat)
            {
                TeamChatPatches.RpcSendVampTeamChat(PlayerControl.LocalPlayer, textRegular);
                MiscUtils.AddTeamChat(PlayerControl.LocalPlayer.Data, $"<color=#{TownOfUsColors.Vampire.ToHtmlStringRGBA()}>{PlayerControl.LocalPlayer.Data.PlayerName} (吸血鬼聊天)</color>", textRegular, onLeft: false);

                __instance.freeChatField.Clear();
                __instance.quickChatMenu.Clear();
                __instance.quickChatField.Clear();
                __instance.UpdateChatMode();

                return false;
            }
            else if (PlayerControl.LocalPlayer.IsImpostor() && genOpt is { FFAImpostorMode: false, ImpostorChat.Value: true })
            {
                TeamChatPatches.RpcSendImpTeamChat(PlayerControl.LocalPlayer, textRegular);
                MiscUtils.AddTeamChat(PlayerControl.LocalPlayer.Data, $"<color=#{TownOfUsColors.ImpSoft.ToHtmlStringRGBA()}>{PlayerControl.LocalPlayer.Data.PlayerName} (内鬼聊天)</color>", textRegular, onLeft: false);

                __instance.freeChatField.Clear();
                __instance.quickChatMenu.Clear();
                __instance.quickChatField.Clear();
                __instance.UpdateChatMode();

                return false;
            }
            return true;
        }
        return true;
    }

    [MethodRpc((uint)TownOfUsRpc.ChangeHost, SendImmediately = true)]
    private static void RpcChangeHost(PlayerControl host, int id, PlayerControl playerCon)
    {
        if (!host.IsHost())
        {
            Logger<TownOfUsPlugin>.Error($"{host.Data.PlayerName} 不是房主");
            return;
        }
        else if (id == -1)
        {
            Logger<TownOfUsPlugin>.Error($"无效的客户端ID: {id}");
            return;
        }
        AmongUsClient.Instance.HostId = id;
        DoHostSetup();
		GameStartManager.Instance.StartCoroutine(GameStartManager.Instance.HostInfoPanel.SetCosmetics(playerCon.Data));
    }
	internal static void DoHostSetup()
	{
        var manager = GameStartManager.Instance;
		string text = InnerNet.GameCode.IntToGameName(AmongUsClient.Instance.GameId);
		if (!AmongUsClient.Instance.AmHost)
		{
			manager.HostPrivacyButtons.gameObject.SetActive(false);
			manager.ClientPrivacyValue.gameObject.SetActive(true);
			manager.StartButton.gameObject.SetActive(false);
			manager.StartButtonClient.gameObject.SetActive(true);
			manager.GameStartTextParent.SetActive(false);
			manager.HostInfoPanelButtons.gameObject.SetActive(false);
			manager.ClientInfoPanelButtons.gameObject.SetActive(true);
			return;
		}
		if (text != null)
		{
			manager.HostPrivacyButtons.gameObject.SetActive(true);
			manager.ClientPrivacyValue.gameObject.SetActive(false);
		}
        AmongUsClient.Instance.OnBecomeHost();
		manager.HostInfoPanelButtons.gameObject.SetActive(true);
		manager.ClientInfoPanelButtons.gameObject.SetActive(false);
		manager.StartButton.gameObject.SetActive(true);
		manager.StartButtonClient.gameObject.SetActive(false);
	}
}
