using System;
using System.Globalization;
using System.Windows.Data;

namespace ArmSoft.SME.Core.UI.DataViews;

public class TotalSummaryConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return $"TotalSummary_{value}";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
