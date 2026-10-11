using System;
using System.Globalization;
using System.Windows.Data;
using ArmSoft.SME.Common;
using DevExpress.Data;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Converters;

internal class SummaryTypeTyText : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		GridSummaryItem gridSummaryItem = (GridSummaryItem)value;
		if (gridSummaryItem.SummaryType == SummaryItemType.Sum)
		{
			return "";
		}
		return gridSummaryItem.SummaryType switch
		{
			SummaryItemType.Average => (object)Resources.SummaryAvg, 
			SummaryItemType.Count => Resources.SummaryCnt, 
			SummaryItemType.Max => Resources.SummaryMax, 
			SummaryItemType.Min => Resources.SummaryMin, 
			_ => gridSummaryItem.SummaryType.ToString(), 
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
