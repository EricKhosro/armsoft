using System;
using System.Globalization;
using System.Windows.Data;

namespace ArmSoft.SME.Trade.UI.Converters;

public class TextWithTwoDotsConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return $"{parameter}: ";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
