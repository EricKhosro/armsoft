using System;
using System.Globalization;
using System.Windows.Data;
using ArmSoft.SME.Common;

namespace ArmSoft.SME.Core.UI.Converters;

public class GetFavoritesText : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return Resources.RibbonPageFavorites;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
