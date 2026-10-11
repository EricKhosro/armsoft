using System;
using System.Globalization;
using System.Windows.Data;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.Converters;

public class GuidToImage : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null || (Guid)value == Guid.Empty)
		{
			return null;
		}
		if (!string.IsNullOrWhiteSpace(System.Convert.ToString(parameter)))
		{
			short value2 = SmartEnumHelper.GetValue<short>(typeof(ImageItemTypeSE), System.Convert.ToString(parameter));
			if (value2 != 0)
			{
				return Settings.ImagesService.GetItemImage(System.Convert.ToInt16(value2), System.Convert.ToString(value));
			}
			return null;
		}
		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
