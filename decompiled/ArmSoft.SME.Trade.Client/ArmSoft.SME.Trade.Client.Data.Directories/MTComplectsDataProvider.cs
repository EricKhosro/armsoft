using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class MTComplectsDataProvider : DataProvider<MTComplectsDataProvider.DataRow, MTComplectsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public string Description { get; set; }
	}

	public class Param
	{
		public string Group { get; set; }

		public string ComplectCode { get; set; }
	}

	public override string Name => "MTComplects";

	public MTComplectsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
