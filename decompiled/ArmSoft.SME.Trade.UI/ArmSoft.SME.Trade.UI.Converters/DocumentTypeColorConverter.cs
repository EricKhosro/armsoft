using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Converters;

public class DocumentTypeColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		short num = System.Convert.ToInt16(value);
		if (num == DocumentTypeSE.MTBill.Value)
		{
			if (!Settings.IsDarkTheme)
			{
				return (SolidColorBrush)new BrushConverter().ConvertFrom("#3E7337");
			}
			return (SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77");
		}
		if (num == DocumentTypeSE.MTBillReturn.Value || num == DocumentTypeSE.MTReturnFromCustomer.Value || num == DocumentTypeSE.MTReturnToSupplier.Value)
		{
			if (!Settings.IsDarkTheme)
			{
				return (SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326");
			}
			return (SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376");
		}
		if (!Settings.IsDarkTheme)
		{
			return (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
		}
		return (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
