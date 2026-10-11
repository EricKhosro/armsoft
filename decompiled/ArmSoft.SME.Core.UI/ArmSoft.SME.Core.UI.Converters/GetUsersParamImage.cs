using System;
using System.Globalization;
using System.Windows.Data;

namespace ArmSoft.SME.Core.UI.Converters;

public class GetUsersParamImage : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if (parameter != null)
		{
			UsersParamButton usersParamButton = (UsersParamButton)parameter;
			if (usersParamButton.ConnectedControl != null)
			{
				usersParamButton.Height = ((usersParamButton.ConnectedControl.ActualHeight > 0.0) ? usersParamButton.ConnectedControl.ActualHeight : 20.0);
			}
		}
		return ImageService.GetImage("users_16.svg");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
