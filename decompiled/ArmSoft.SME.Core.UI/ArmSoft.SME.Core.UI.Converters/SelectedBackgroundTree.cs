using System;
using System.Globalization;
using System.Windows.Data;
using DevExpress.Data;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Converters;

public class SelectedBackgroundTree : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is TreeListSummaryItem treeListSummaryItem))
		{
			return Settings.GetSelectedRowBrush();
		}
		if (treeListSummaryItem.SummaryType == SummaryItemType.Custom)
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
