using System;
using System.Globalization;
using System.Windows.Data;
using DevExpress.Data;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Converters;

internal class LabelWidthBySummaryType : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		GridSummaryItem gridSummaryItem = (GridSummaryItem)value;
		if (gridSummaryItem.SummaryType == SummaryItemType.Sum)
		{
			return 0;
		}
		if (gridSummaryItem.SummaryType == SummaryItemType.Custom)
		{
			return 0;
		}
		return 30;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
