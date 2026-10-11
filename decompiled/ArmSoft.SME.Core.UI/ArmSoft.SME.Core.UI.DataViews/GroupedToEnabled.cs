using System;
using System.Globalization;
using System.Windows.Data;

namespace ArmSoft.SME.Core.UI.DataViews;

public class GroupedToEnabled : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (System.Convert.ToInt32(value) == 0)
		{
			return false;
		}
		return true;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
