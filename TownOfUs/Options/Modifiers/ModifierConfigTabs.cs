using MiraAPI.GameOptions;
using UnityEngine;

namespace TownOfUs.Options.Modifiers;

public sealed class AllianceOptions : AbstractOptionGroup
{
    public override string GroupName => "联盟配置";
    public override Color GroupColor => Color.white;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 10;
}

public sealed class UniversalOptions : AbstractOptionGroup
{
    public override string GroupName => "通用附加特性配置";
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 20;
}
public sealed class CrewOptions : AbstractOptionGroup
{
    public override string GroupName => "船员附加特性配置";
    public override Color GroupColor => Palette.CrewmateRoleHeaderBlue;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 30;
}

public sealed class ImpostorOptions : AbstractOptionGroup
{
    public override string GroupName => "伪装者附加特性配置";
    public override Color GroupColor => Palette.ImpostorRoleHeaderRed;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 40;
}

public sealed class NeutralOptions : AbstractOptionGroup
{
    public override string GroupName => "中立附加特性配置";
    public override Color GroupColor => TownOfUsColors.Neutral;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 50;
}