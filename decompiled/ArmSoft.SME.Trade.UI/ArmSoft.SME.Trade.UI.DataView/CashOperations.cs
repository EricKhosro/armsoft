using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10055)]
public class CashOperations : CashBankOperationsBase<CashBankOperationsDataProvider.DataRow, CashBankOperationsDataProvider.Param>
{
	public override string HelpId => "Purchases_Reports_CashBankReports_CashOperations";

	public CashOperations()
		: base(true)
	{
		DataProvider = new CashOperationsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}
}
