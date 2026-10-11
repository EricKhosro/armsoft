using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ArmSoft.SME.Common;

namespace ArmSoft.SME.Core.UI.Converters;

public class SystemSpecificCheckedButtonBackgroundColor : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return Settings.SystemType switch
		{
			SystemTypes.Trade => (object)new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#3e7337")), 
			SystemTypes.SalesManagement => new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#F58021")), 
			_ => new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#296ec6")), 
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
