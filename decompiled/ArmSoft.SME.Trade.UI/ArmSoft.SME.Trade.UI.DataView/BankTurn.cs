using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10063)]
public class BankTurn : CashBankTurnBase
{
	public override string HelpId => "Purchases_Reports_CashBankReports_BankTurnOver";

	public BankTurn()
	{
		DataProvider = new BankTurnOverDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}
}
