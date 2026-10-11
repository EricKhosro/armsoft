using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10047)]
public class DiscountLogsDataView : TradeDataView<DiscountLogsDataProvider.DataRow, DiscountLogsDataProvider.Param>
{
	public override string HelpId => "Sales_Reports_DiscountsCalculationData";

	public DiscountLogsDataView()
	{
		DataProvider = new DiscountLogsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}
}
