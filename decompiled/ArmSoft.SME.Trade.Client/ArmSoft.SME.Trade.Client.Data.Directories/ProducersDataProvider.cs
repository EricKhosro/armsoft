using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class ProducersDataProvider : CodeCaptionPairsDataProvider
{
	public override string Name => "Producers";

	public ProducersDataProvider(TradeClient client)
		: base(client)
	{
	}
}
