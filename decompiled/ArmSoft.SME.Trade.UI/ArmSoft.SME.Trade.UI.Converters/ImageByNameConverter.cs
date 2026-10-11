using System;
using System.Globalization;
using System.Windows.Data;
using ArmSoft.SME.Core.UI;

namespace ArmSoft.SME.Trade.UI.Converters;

public class ImageByNameConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return ImageService.GetImage(parameter.ToString());
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
