using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;

namespace ArmSoft.SME.Trade.UI.DataView.Reports.Analysis;

[DataView(10087)]
public class PartnersABCAnalysisDataView : ABCAnalysisDataView
{
	public PartnersABCAnalysisDataView()
		: base(analyzeByPartners: true)
	{
		DataProvider = new PartnersABCAnalysisDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}
}
