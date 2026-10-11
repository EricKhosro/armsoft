using System;
using System.Globalization;
using System.Windows.Data;

namespace ArmSoft.SME.Core.UI.Converters;

public class ConvertStringToImage : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return ImageService.GetImage(value.ToString().Replace("pack://application:,,,/ArmSoft.SME.Core.UI;component/Images/", ""));
		}
		catch (Exception)
		{
			return null;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
