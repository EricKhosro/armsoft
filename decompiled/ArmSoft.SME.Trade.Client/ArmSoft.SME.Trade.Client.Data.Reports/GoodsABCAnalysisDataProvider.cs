namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class GoodsABCAnalysisDataProvider : ABCAnalysisDataProvider
{
	public override string Name => "GoodsABCAnalysis";

	public GoodsABCAnalysisDataProvider(TradeClient client)
		: base(client)
	{
	}
}
