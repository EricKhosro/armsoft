using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10065)]
public class BankOperations : CashBankOperationsBase<CashBankOperationsDataProvider.DataRow, CashBankOperationsDataProvider.Param>
{
	public override string HelpId => "Purchases_Reports_CashBankReports_BankOperations";

	public BankOperations()
		: base(false)
	{
		DataProvider = new BankOperationsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}
}
