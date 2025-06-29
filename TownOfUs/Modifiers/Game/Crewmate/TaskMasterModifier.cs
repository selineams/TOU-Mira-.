using Il2CppSystem.Text;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using System.Text.RegularExpressions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Crewmate;

public sealed class TaskmasterModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "任务大师";
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Taskmaster;
    public override string GetDescription() => "每次会议后会自动完成一个随机任务";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePassive;
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.TaskmasterChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.TaskmasterAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate() && !(GameOptionsManager.Instance.currentNormalGameOptions.MapId is 4 or 6);
    }

    public void OnRoundStart()
    {
        if (Player.AmOwner && Player.myTasks.Count > 0 && !Player.HasDied())
        {
            var tasks = Player.myTasks.ToArray().Where(x => x.TryCast<NormalPlayerTask>() != null && !x.IsComplete).ToList();

            if (tasks.Count > 0)
            {
                tasks.Shuffle();

                var randomTask = tasks[0];

                HudManager.Instance.ShowTaskComplete();
                Player.RpcCompleteTask(randomTask.Id);

                var sb = new StringBuilder();
                randomTask.AppendTaskText(sb);

                var pattern = $@" \(.*?\)";
                var query = sb.ToString();
                var taskText = Regex.Replace(query, pattern, string.Empty);
                taskText = taskText.Replace(Environment.NewLine, "");

                var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.Taskmaster.ToTextColor()}任务“{taskText}”已为你自动完成。</b></color>", Color.white, spr: TouModifierIcons.Taskmaster.LoadAsset());
                notif1.Text.SetOutlineThickness(0.35f);
                notif1.transform.localPosition = new Vector3(0f, 1f, -20f);
            }
        }
    }
    public string GetAdvancedDescription()
    {
        return "每回合开始时，你会自动完成一个任务。";
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
