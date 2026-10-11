using System;
using System.Globalization;
using System.Windows.Data;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Converters;

public class SelectedBackground : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is GridSummaryItem { CalculationMode: var calculationMode }))
		{
			return Settings.GetSelectedRowBrush();
		}
		if (calculationMode == GridSummaryCalculationMode.SelectedRows)
		{
			return Settings.GetSelectedRowBrush();
		}
		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
