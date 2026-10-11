using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ArmSoft.SME.Core.UI.Converters;

public class NumericValueToIsNegative : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			if (value != null && double.TryParse(value.ToString(), out var result))
			{
				if (result < 0.0)
				{
					if ((object)targetType == typeof(Brush))
					{
						return Settings.EditorErrorBrush;
					}
					return true;
				}
				if ((object)targetType == typeof(Brush))
				{
					return Settings.GetForegroundBrush();
				}
				return false;
			}
			if ((object)targetType == typeof(Brush))
			{
				return Settings.GetForegroundBrush();
			}
			return false;
		}
		catch (Exception)
		{
			if ((object)targetType == typeof(Brush))
			{
				return Settings.GetForegroundBrush();
			}
			return false;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
