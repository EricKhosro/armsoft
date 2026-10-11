using System;
using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;

namespace ArmSoft.SME.Trade.UI.DataView;

public class TradeDataViewParametersDialogWindow : DataViewParametersDialogWindow
{
	public TradeDataViewParametersDialogWindow(IDataView idv, Window owner = null)
		: base(idv, owner)
	{
	}

	public PartnerContractGroupControls AddPartnerContractGroupControls()
	{
		return new PartnerContractGroupControls(this);
	}

	public static bool CheckPeriodForActualness(DateOnly date)
	{
		if (Settings.ParametersService.CheckActualnessDateForReports())
		{
			DateOnly? dateOnly = Settings.ParametersService.ActualnessDate();
			if (date > dateOnly)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.CheckReportActualnessDate, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
				{
					return false;
				}
				return true;
			}
		}
		return true;
	}
}
