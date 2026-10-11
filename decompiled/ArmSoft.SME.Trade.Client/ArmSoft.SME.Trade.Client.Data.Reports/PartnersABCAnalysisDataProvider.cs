namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class PartnersABCAnalysisDataProvider : ABCAnalysisDataProvider
{
	public override string Name => "PartnersABCAnalysis";

	public PartnersABCAnalysisDataProvider(TradeClient client)
		: base(client)
	{
	}
}
