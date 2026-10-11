using System;
using System.Globalization;
using System.Windows.Data;
using DevExpress.Data;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Converters;

internal class LabelWidthBySummaryTypeTree : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		TreeListSummaryItem treeListSummaryItem = (TreeListSummaryItem)value;
		if (treeListSummaryItem.SummaryType == SummaryItemType.Sum)
		{
			return 0;
		}
		if (treeListSummaryItem.SummaryType == SummaryItemType.Custom)
		{
			return 0;
		}
		return 35;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
