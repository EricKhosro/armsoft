using System;
using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.Documents;

namespace ArmSoft.SME.Trade.UI.Helpers;

public static class ActualnessDateHelpers
{
	public static void IsActualnessDateCorrect()
	{
		bool isActualnessDateCorrect = TradeSettings.CreateTradeClient().CommonServices.InnerIsActualnessDateCorrect();
		Settings.HostingEnvironment.RefreshActualnessDateStatusBar(isActualnessDateCorrect);
	}

	public static void ChangeActualnessDate()
	{
		if (!Settings.LoginService.IsLoggedIn || !Settings.LoginService.IsAdmin)
		{
			return;
		}
		try
		{
			Settings.ParametersService.AllowLogin_set(value: false);
			ChangeActualnessDateWindow changeActualnessDateWindow = new ChangeActualnessDateWindow();
			if (changeActualnessDateWindow.ShowDialog().Value)
			{
				if (changeActualnessDateWindow.ResultDialog)
				{
					IsActualnessDateCorrect();
					ArmSoft.SME.Core.UI.MessageBox.Show(Resources.ChangeActualnessDateSucc, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				}
				else
				{
					IsActualnessDateCorrect();
					if (changeActualnessDateWindow.ReportErrorList != null && changeActualnessDateWindow.ReportErrorList.RowsCount > 0)
					{
						changeActualnessDateWindow.ReportErrorList.Show();
					}
				}
			}
			Settings.ParametersService.AllowLogin_set(value: true);
			Settings.ParametersService.Refresh();
		}
		catch (Exception ex)
		{
			Settings.ParametersService.AllowLogin_set(value: true);
			Settings.ParametersService.Refresh();
			Settings.ErrorHandler(ex);
		}
	}
}
