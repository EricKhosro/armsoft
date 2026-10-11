using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class SegmentsDataProvider : CodeCaptionPairsDataProvider
{
	public override string Name => "Segments";

	public SegmentsDataProvider(TradeClient client)
		: base(client)
	{
	}
}
