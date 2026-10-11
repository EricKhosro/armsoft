using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10056)]
public class CashRemains : CashBankRemainsBase<CashBankRemainsDataProvider.DataRow, CashBankRemainsDataProvider.Param>
{
	public override string HelpId => "Purchases_Reports_CashBankReports_CashBalances";

	public CashRemains()
		: base(true)
	{
		DataProvider = new CashRemainsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}
}
