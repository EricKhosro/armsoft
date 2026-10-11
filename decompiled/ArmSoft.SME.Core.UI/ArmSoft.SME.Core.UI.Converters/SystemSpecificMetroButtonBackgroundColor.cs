using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ArmSoft.SME.Common;

namespace ArmSoft.SME.Core.UI.Converters;

public class SystemSpecificMetroButtonBackgroundColor : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return Settings.SystemType switch
		{
			SystemTypes.Trade => (object)(Settings.IsDarkTheme ? new SolidColorBrush(Color.FromArgb(byte.MaxValue, 89, 173, 119)) : Settings.GetApplicationBrush()), 
			SystemTypes.Accountant => Settings.IsDarkTheme ? new SolidColorBrush(Color.FromArgb(byte.MaxValue, 128, 195, 250)) : Settings.GetApplicationBrush(), 
			SystemTypes.SalesManagement => Settings.GetApplicationBrush(), 
			_ => Settings.GetApplicationBrush(), 
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
