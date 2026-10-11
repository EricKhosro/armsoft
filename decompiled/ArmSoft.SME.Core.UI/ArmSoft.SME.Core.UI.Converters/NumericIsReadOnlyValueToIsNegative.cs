using System;
using System.Globalization;
using System.Windows.Data;

namespace ArmSoft.SME.Core.UI.Converters;

public class NumericIsReadOnlyValueToIsNegative : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values != null)
		{
			bool flag = false;
			if (values[1] is bool flag2)
			{
				flag = flag2;
			}
			if (values[0] is decimal num && num < 0m)
			{
				if (!flag)
				{
					return Settings.EditorErrorBrush;
				}
				return Settings.ReadOnlyEditorErrorBrush;
			}
			if (values[0] is int num2 && num2 < 0)
			{
				if (!flag)
				{
					return Settings.EditorErrorBrush;
				}
				return Settings.ReadOnlyEditorErrorBrush;
			}
			if (values[0] is short num3 && num3 < 0)
			{
				if (!flag)
				{
					return Settings.EditorErrorBrush;
				}
				return Settings.ReadOnlyEditorErrorBrush;
			}
			if (flag)
			{
				return Settings.ReadOnlyBrush;
			}
			return Settings.GetForegroundBrush();
		}
		return Settings.GetForegroundBrush();
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
