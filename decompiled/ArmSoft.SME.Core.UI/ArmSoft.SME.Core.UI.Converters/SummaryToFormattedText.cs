using System;
using System.Globalization;
using System.Windows.Data;
using DevExpress.Data;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Converters;

internal class SummaryToFormattedText : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		GridTotalSummaryData gridTotalSummaryData = (GridTotalSummaryData)value;
		object obj = gridTotalSummaryData.Value;
		if (obj == null && (gridTotalSummaryData.Column.UnboundType == UnboundColumnType.Decimal || gridTotalSummaryData.Column.UnboundType == UnboundColumnType.Integer))
		{
			obj = 0;
		}
		if (gridTotalSummaryData.Item.SummaryType == SummaryItemType.Count)
		{
			return (int)obj;
		}
		if (gridTotalSummaryData.Column.EditSettings != null && !string.IsNullOrWhiteSpace(gridTotalSummaryData.Column.EditSettings.DisplayFormat))
		{
			return string.Format(gridTotalSummaryData.Column.EditSettings.DisplayFormat, obj);
		}
		if (obj == null)
		{
			return "";
		}
		return obj.ToString();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
