using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class StandardCommentsDataProvider : CodeCaptionPairsDataProvider
{
	public override string Name => "StandardComments";

	public StandardCommentsDataProvider(TradeClient client)
		: base(client)
	{
	}
}
