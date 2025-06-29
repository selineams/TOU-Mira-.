using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Impostor;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Game.Impostor;

public sealed class TelepathModifier : TouGameModifier, IWikiDiscoverable
{
    public override string ModifierName => "通感者";
    public override string GetDescription() =>
            (OptionGroupSingleton<TelepathOptions>.Instance.KnowKillLocation ? "知晓队友击杀的时间和位置" : "知晓队友击杀的时间")
            + (OptionGroupSingleton<TelepathOptions>.Instance.KnowDeath && !OptionGroupSingleton<TelepathOptions>.Instance.KnowDeathLocation ? "，知晓队友死亡的时间" : string.Empty)
            + (OptionGroupSingleton<TelepathOptions>.Instance.KnowDeath && OptionGroupSingleton<TelepathOptions>.Instance.KnowDeathLocation ? "，知晓队友死亡的时间和位置。" : string.Empty);
    public override LoadableAsset<Sprite>? ModifierIcon => TouModifierIcons.Telepath;
    public override ModifierFaction FactionType => ModifierFaction.ImpostorPostmortem;

    public float Timer { get; set; }

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.TelepathChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.TelepathAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsImpostor() && PlayerControl.AllPlayerControls.ToArray().Count(x => x.IsImpostor() && !x.HasDied()) != 1;
    }
    public string GetAdvancedDescription()
    {
        var options = OptionGroupSingleton<TelepathOptions>.Instance;
        return
            (options.KnowKillLocation ? "知晓队友击杀的时间和位置" : "知晓队友击杀的时间")
            + (options.KnowDeath && !options.KnowDeathLocation ? "，知晓队友死亡的时间。" : string.Empty)
            + (options.KnowDeath && options.KnowDeathLocation ? "，知晓队友死亡的时间和位置。" : string.Empty)
               + MiscUtils.AppendOptionsText(GetType());
    }

    public List<CustomButtonWikiDescription> Abilities { get; } = [];
}
