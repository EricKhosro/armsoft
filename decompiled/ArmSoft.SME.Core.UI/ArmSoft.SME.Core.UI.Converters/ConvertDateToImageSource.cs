using System;
using System.Globalization;
using System.Windows.Data;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.Converters;

public class ConvertDateToImageSource : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if (System.Convert.ToString(parameter) == "Small")
		{
			return ImageService.GetImage(ASImageHelper.CombineDayDateCalendarSmallImageUri(Settings.HostingEnvironment.CurrentDate), isAppPathPreferred: false, "Calendar/");
		}
		return ImageService.GetImage(ASImageHelper.CombineDayDateCalendarLargeImageUri(Settings.HostingEnvironment.CurrentDate), isAppPathPreferred: false, "Calendar/");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
