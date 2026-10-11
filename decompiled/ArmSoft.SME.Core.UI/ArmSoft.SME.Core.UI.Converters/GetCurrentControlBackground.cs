using System;
using System.Globalization;
using System.Windows.Data;

namespace ArmSoft.SME.Core.UI.Converters;

public class GetCurrentControlBackground : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return Settings.GetControlBackground();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
