using System;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.SmartEnums;

public class FeatureStatusTypeSE : ASSmartEnum<FeatureStatusTypeSE, byte>
{
	public static readonly FeatureStatusTypeSE Regular = new FeatureStatusTypeSE("Regular", 0, () => "", Brushes.Transparent);

	public static readonly FeatureStatusTypeSE New = new FeatureStatusTypeSE("New", 1, () => Resources.NewForAddedFeature, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EC6206")));

	public static readonly FeatureStatusTypeSE Updated = new FeatureStatusTypeSE("Updated", 2, () => Resources.UpdatedFeature, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#17AB4F")));

	public Brush BadgeColor { get; }

	private FeatureStatusTypeSE(string name, byte value, Func<string> caption, Brush badgeColor)
		: base(name, value, caption)
	{
		BadgeColor = badgeColor;
	}
}
