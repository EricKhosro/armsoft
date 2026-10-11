using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ArmSoft.SME.Core.UI.Converters;

public class AsDetailsRowConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is Dictionary<int, List<object>> dictionary)
		{
			if (dictionary.TryGetValue((int)value, out var value2))
			{
				return value2;
			}
			return null;
		}
		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
