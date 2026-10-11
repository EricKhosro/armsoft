using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;

namespace ArmSoft.SME.Trade.UI.DataView.Reports.Analysis;

[DataView(10086)]
public class GoodsABCAnalysisDataView : ABCAnalysisDataView
{
	public GoodsABCAnalysisDataView()
	{
		DataProvider = new GoodsABCAnalysisDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}
}
