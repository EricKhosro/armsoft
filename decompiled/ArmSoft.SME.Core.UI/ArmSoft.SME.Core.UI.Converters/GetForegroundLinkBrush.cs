using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ArmSoft.SME.Core.UI.Converters;

public class GetForegroundLinkBrush : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (Settings.IsDarkTheme)
		{
			return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#eaa1ea"));
		}
		return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#8a398a"));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
