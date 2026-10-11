using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10064)]
public class BankRemains : CashBankRemainsBase<CashBankRemainsDataProvider.DataRow, CashBankRemainsDataProvider.Param>
{
	public override string HelpId => "Purchases_Reports_CashBankReports_BankRemains";

	public BankRemains()
		: base(false)
	{
		DataProvider = new BankRemainsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}
}
