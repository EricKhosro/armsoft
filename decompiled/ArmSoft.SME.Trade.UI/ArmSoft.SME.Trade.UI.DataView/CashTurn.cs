using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10062)]
public class CashTurn : CashBankTurnBase
{
	public override string HelpId => "Purchases_Reports_CashBankReports_CashTurnOver";

	public CashTurn()
		: base(isCashDeskTurn: true)
	{
		DataProvider = new CashTurnOverDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}
}
