using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.Directories;

namespace ArmSoft.SME.Trade.UI.DataView;

public class UsersWorkingPeriods : TradeDataView<UsersWorkingPeriodsDataProvider.DataRow, UsersWorkingPeriodsDataProvider.Param>
{
	public override string HelpId => "AdminTools_DBConnections";

	public UsersWorkingPeriods(string userId)
	{
		ParametersProvider.UserID = userId;
		DataProvider = new UsersWorkingPeriodsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("PeriodStart");
		}
	}
}
