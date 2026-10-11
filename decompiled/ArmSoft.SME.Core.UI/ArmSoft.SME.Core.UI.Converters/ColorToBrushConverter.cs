using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.Converters;

public class ColorToBrushConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (System.Convert.ToInt32(parameter) == 1)
		{
			return new SolidColorBrush((Color)value);
		}
		if (Settings.HostingEnvironment.gUserColor == null)
		{
			if (ApplicationThemeHelper.ApplicationThemeName.Contains("Dark"))
			{
				Settings.HostingEnvironment.gUserColor = new SolidColorBrush(Colors.DarkRed);
			}
			else if (ApplicationThemeHelper.ApplicationThemeName.Contains("Black") || ApplicationThemeHelper.ApplicationThemeName == "Office2010Silver")
			{
				Settings.HostingEnvironment.gUserColor = new SolidColorBrush(Colors.Orange);
			}
			else
			{
				Settings.HostingEnvironment.gUserColor = new SolidColorBrush(Colors.Yellow);
			}
		}
		return Settings.HostingEnvironment.gUserColor;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
