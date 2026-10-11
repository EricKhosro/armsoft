using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Models;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Data;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10002)]
public class AccountInBanks : TradeDirectoryDataView<ArmSoft.SME.Trade.Client.Models.AccountInBanks, AccountInBanksDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.AccountInBank.Value;

	public override string HelpId => "Purchases_Lists_Otherlists_AccountsInBank";

	public AccountInBanks()
	{
		DataProvider = new AccountInBanksDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("IsDefault", ColumnSortOrder.Descending);
			dvp.Set_ColumnSortOrder("Code");
		}
	}
}
