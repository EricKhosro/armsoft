using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ArmSoft.SME.Trade.UI.DataView.Reports.Analysis;

public class PanelStateConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = System.Convert.ToBoolean(values[0]);
		int num = System.Convert.ToInt32(values[1]);
		switch (System.Convert.ToString(parameter) ?? "")
		{
		case "Maximize":
			if (flag)
			{
				return Visibility.Visible;
			}
			if (num < 3)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		case "Restore":
			if (flag)
			{
				return Visibility.Collapsed;
			}
			if (num == 3)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		case "Hide":
			if (flag)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		default:
			if (flag)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotFiniteNumberException();
	}
}
