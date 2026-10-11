using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ArmSoft.SME.Core.UI.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return Visibility.Hidden;
		}
		if ((bool)value)
		{
			return Visibility.Visible;
		}
		return Visibility.Hidden;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
