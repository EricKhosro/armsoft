using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Converters;

internal class SummaryToFontWeight : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (((GridSummaryItem)value).CalculationMode == GridSummaryCalculationMode.SelectedRows)
		{
			return FontWeights.Bold;
		}
		return FontWeights.Normal;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
