using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class SubstituteItemsGroupsDataProvider : CodeCaptionPairsDataProvider
{
	public override string Name => "SubstituteitemsGroups";

	public SubstituteItemsGroupsDataProvider(TradeClient client)
		: base(client)
	{
	}
}
