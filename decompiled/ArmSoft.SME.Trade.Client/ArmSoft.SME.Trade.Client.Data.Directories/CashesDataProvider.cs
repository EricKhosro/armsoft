using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Trade.Client.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class CashesDataProvider : DataProvider<Cashes, CashesDataProvider.Param>
{
	public class Param
	{
		public bool CheckUserAccess { get; set; }

		public bool ShowOnlyECRCashes { get; set; }
	}

	public override string Name => "Cashes";

	public CashesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
