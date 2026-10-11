using System.Windows;

namespace ArmSoft.SME.Core.UI.ExceptionDispatch;

public static class ExceptionDispatchHelper
{
	public static bool? ShowDialogWithExceptionHandling<T>(this T window) where T : Window, IExceptionDispatchHandler
	{
		bool? result = window.ShowDialog();
		if (window.ExceptionDispatchInfo != null)
		{
			window.ExceptionDispatchInfo.Throw();
		}
		return result;
	}
}
