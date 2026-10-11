using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ArmSoft.SME.Core.UI.Converters;

public class HyperlinkBackgroundColor : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!Settings.IsDarkTheme)
		{
			return new SolidColorBrush(Color.FromArgb(byte.MaxValue, 51, 122, 183));
		}
		return new SolidColorBrush(Color.FromArgb(byte.MaxValue, 128, 195, 250));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
