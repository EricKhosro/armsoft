using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Trade.Client.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class AccountInBanksDataProvider : DataProvider<AccountInBanks, AccountInBanksDataProvider.Param>
{
	public class Param
	{
		public string CurrCode { get; set; }

		public string BankingMode { get; set; }
	}

	public override string Name => "AccountInBanks";

	public AccountInBanksDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
