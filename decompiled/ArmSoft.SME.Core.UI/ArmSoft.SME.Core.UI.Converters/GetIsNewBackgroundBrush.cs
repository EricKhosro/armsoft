using System;
using System.Globalization;
using System.Windows.Data;
using ArmSoft.SME.Core.UI.SmartEnums;

namespace ArmSoft.SME.Core.UI.Converters;

public class GetIsNewBackgroundBrush : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return FeatureStatusTypeSE.New.BadgeColor;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
