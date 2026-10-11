using System;
using System.Globalization;
using System.Windows.Data;
using ArmSoft.SME.Core.UI.SmartEnums;

namespace ArmSoft.SME.Core.UI.Converters;

public class GetFeatureStatusBackgroundBrush : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return null;
		}
		return ((FeatureStatusTypeSE)value).BadgeColor;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
