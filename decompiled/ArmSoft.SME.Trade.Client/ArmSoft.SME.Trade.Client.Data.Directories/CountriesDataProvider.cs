using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class CountriesDataProvider : CodeCaptionPairsDataProvider
{
	public override string Name => "Countries";

	public CountriesDataProvider(TradeClient client)
		: base(client)
	{
	}
}
