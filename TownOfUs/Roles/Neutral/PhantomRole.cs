using System.Collections;
using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfUs.Modifiers;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfUs.Roles.Neutral;

public sealed class PhantomTouRole(IntPtr cppPtr) : NeutralGhostRole(cppPtr), ITownOfUsRole, IGhostRole, IWikiDiscoverable
{
    public override string RoleName => "幻影";
    public override string RoleDescription => string.Empty;
    public override string RoleLongDescription => "在不被抓到的情况下完成你所有的任务！";
    public override Color RoleColor => TownOfUsColors.Phantom;
    public override RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    public override CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Phantom,
        HideSettings = false,
        ShowInFreeplay = true,
    };

    public bool Setup { get; set; }
    public bool Caught { get; set; }
    public bool Faded { get; set; }
    public bool CanBeClicked { get; set; }
    public bool CompletedAllTasks { get; private set; }
    public bool GhostActive => Setup && !Caught;

    public bool MetWinCon => CompletedAllTasks;

    public override void UseAbility()
    {
        if (GhostActive) return;

        base.UseAbility();
    }

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        if (TutorialManager.InstanceExists)
        {
            Setup = true;

            Coroutines.Start(SetTutorialCollider(Player));

            if (Player.AmOwner)
            {
                Player.MyPhysics.ResetMoveState();

                HudManager.Instance.SetHudActive(false);
                HudManager.Instance.SetHudActive(true);
                HudManager.Instance.AbilityButton.SetDisabled();
                Patches.HudManagerPatches.ResetZoom();
            }
        }
        MiscUtils.AdjustGhostTasks(player);
    }
    private static IEnumerator SetTutorialCollider(PlayerControl player)
    {
        yield return new WaitForSeconds(0.01f);

        player.gameObject.layer = LayerMask.NameToLayer("Players");

        player.gameObject.GetComponent<PassiveButton>().OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
        player.gameObject.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() => player.OnClick()));
        player.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        if (TutorialManager.InstanceExists)
        {
            Player.ResetAppearance();
            Player.cosmetics.ToggleNameVisible(true);

            Player.cosmetics.currentBodySprite.BodySprite.color = Color.white;
            Player.gameObject.layer = LayerMask.NameToLayer("Ghost");
            Player.MyPhysics.ResetMoveState();

            Faded = false;
        }
    }
    public override bool CanUse(IUsable console)
    {
        var validUsable = console.TryCast<Console>() ||
            console.TryCast<DoorConsole>() ||
            console.TryCast<OpenDoorConsole>() ||
            console.TryCast<DeconControl>() ||
            console.TryCast<PlatformConsole>() ||
            console.TryCast<Ladder>() ||
            console.TryCast<ZiplineConsole>();

        return GhostActive && validUsable;
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return CompletedAllTasks;
    }

    public override bool WinConditionMet()
    {
        return OptionGroupSingleton<PhantomOptions>.Instance.PhantomWin is PhantomWinOptions.EndsGame && CompletedAllTasks;
    }

    public bool CanCatch() => true;

    public void Spawn()
    {
        Setup = true;

        // Logger<TownOfUsPlugin>.Error($"Setup PhantomTouRole '{Player.Data.PlayerName}'");
        Player.gameObject.layer = LayerMask.NameToLayer("Players");

        Player.gameObject.GetComponent<PassiveButton>().OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
        Player.gameObject.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() => Player.OnClick()));
        Player.gameObject.GetComponent<BoxCollider2D>().enabled = true;

        if (Player.AmOwner)
        {
            Player.SpawnAtRandomVent();
            Player.MyPhysics.ResetMoveState();

            HudManager.Instance.SetHudActive(false);
            HudManager.Instance.SetHudActive(true);
            HudManager.Instance.AbilityButton.SetDisabled();
            Patches.HudManagerPatches.ResetZoom();
        }
    }

    public void FadeUpdate(HudManager instance)
    {
        if (!Caught && Setup)
        {
            Player.GhostFade();
            Faded = true;
        }
        else if (Faded)
        {
            Player.ResetAppearance();
            Player.cosmetics.ToggleNameVisible(true);

            Player.cosmetics.currentBodySprite.BodySprite.color = Color.white;
            Player.gameObject.layer = LayerMask.NameToLayer("Ghost");
            Player.MyPhysics.ResetMoveState();

            Faded = false;

            // Logger<TownOfUsPlugin>.Message($"PhantomTouRole.FadeUpdate UnFaded");
        }
    }

    public void Clicked()
    {
        // Logger<TownOfUsPlugin>.Message($"PhantomTouRole.Clicked");
        Caught = true;
        Player.Exiled();

        if (Player.AmOwner)
        {
            HudManager.Instance.AbilityButton.SetEnabled();
        }
    }

    public void CheckTaskRequirements()
    {
        if (Caught) return;

        var completedTasks = Player.myTasks.ToArray().Count(t => t.IsComplete);
        var tasksRemaining = Player.myTasks.Count - completedTasks;

        CanBeClicked = tasksRemaining <= (int)OptionGroupSingleton<PhantomOptions>.Instance.NumTasksLeftBeforeClickable;
        if (tasksRemaining == (int)OptionGroupSingleton<PhantomOptions>.Instance.NumTasksLeftBeforeClickable && Player.AmOwner)
        {
            var notif1 = Helpers.CreateAndShowNotification($"<b>{TownOfUsColors.Phantom.ToTextColor()}你现在可以被玩家点击了！</b></color>", Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Phantom.LoadAsset());
            notif1.Text.SetOutlineThickness(0.35f);
        }
        CompletedAllTasks = completedTasks == Player.myTasks.Count;

        if (OptionGroupSingleton<PhantomOptions>.Instance.PhantomWin is not PhantomWinOptions.Spooks || !CompletedAllTasks) return;
        if (!Player.AmOwner) return;

        Func<PlayerControl, bool> _playerMatch = plr => !plr.HasDied() && !plr.HasModifier<InvulnerabilityModifier>() && plr != PlayerControl.LocalPlayer;
        var killMenu = CustomPlayerMenu.Create();
        killMenu.transform.FindChild("PhoneUI").GetChild(0).GetComponent<SpriteRenderer>().material = PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
        killMenu.transform.FindChild("PhoneUI").GetChild(1).GetComponent<SpriteRenderer>().material = PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
        killMenu.Begin(
            _playerMatch,
            plr =>
            {
                killMenu.ForceClose();

                if (plr != null)
                {
                    PlayerControl.LocalPlayer.RpcCustomMurder(plr, teleportMurderer: false);
                }
            });
    }
    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return ITownOfUsRole.SetNewTabText(this);
    }

    public string GetAdvancedDescription() 
    {
        return "幻影是一名中立幽灵型角色，只要在被存活玩家点击之前完成所有任务即可获胜。" + MiscUtils.AppendOptionsText(GetType());
    }
}
