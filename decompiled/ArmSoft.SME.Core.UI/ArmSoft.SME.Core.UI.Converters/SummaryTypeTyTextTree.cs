using System;
using System.Globalization;
using System.Windows.Data;
using ArmSoft.SME.Common;
using DevExpress.Data;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Converters;

internal class SummaryTypeTyTextTree : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		TreeListSummaryItem treeListSummaryItem = (TreeListSummaryItem)value;
		if (treeListSummaryItem.SummaryType == SummaryItemType.Sum)
		{
			return string.Empty;
		}
		if (treeListSummaryItem.SummaryType == SummaryItemType.Custom)
		{
			return string.Empty;
		}
		return treeListSummaryItem.SummaryType switch
		{
			SummaryItemType.Average => (object)Resources.SummaryAvg, 
			SummaryItemType.Count => Resources.SummaryCnt, 
			SummaryItemType.Max => Resources.SummaryMax, 
			SummaryItemType.Min => Resources.SummaryMin, 
			_ => treeListSummaryItem.SummaryType.ToString(), 
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
