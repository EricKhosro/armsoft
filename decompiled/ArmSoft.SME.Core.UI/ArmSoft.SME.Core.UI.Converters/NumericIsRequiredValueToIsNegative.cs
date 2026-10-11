using System;
using System.Globalization;
using System.Windows.Data;

namespace ArmSoft.SME.Core.UI.Converters;

public class NumericIsRequiredValueToIsNegative : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values != null)
		{
			bool flag = false;
			bool flag2 = false;
			if (values[0] is bool flag3)
			{
				flag = flag3;
			}
			if (values[1] is bool flag4)
			{
				flag2 = flag4;
			}
			if (flag2 && !flag)
			{
				return Settings.RequiredBrush;
			}
			if (flag)
			{
				return Settings.GetBackgroundBrush();
			}
			return Settings.GetEditorBackground();
		}
		return Settings.GetEditorBackground();
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
