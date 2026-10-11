using System;
using System.Globalization;
using System.Windows.Data;
using ArmSoft.SME.Common;

namespace ArmSoft.SME.Core.UI.Converters;

public class BooleanToString : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if ((bool)value)
		{
			return Resources.Yes;
		}
		return Resources.No;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
