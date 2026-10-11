using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class PartnerAccountsInBankDataProvider : DataProvider<PartnerAccountsInBankDataProvider.DataRow, PartnerAccountsInBankDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public class PartnerAccountsInBankRowKey
		{
			public int PartID { get; set; }

			public string SettlAccount { get; set; }
		}

		public int PartID { get; set; }

		public string PartCode { get; set; }

		public string PartCaption { get; set; }

		public string SettlAccount { get; set; }

		public string Caption { get; set; }

		public bool IsDefault { get; set; }

		public object GetKey()
		{
			return new PartnerAccountsInBankRowKey
			{
				PartID = PartID,
				SettlAccount = SettlAccount
			};
		}

		public string GetCode()
		{
			return "";
		}

		public string GetName()
		{
			return "";
		}

		public bool Matched(object key)
		{
			PartnerAccountsInBankRowKey partnerAccountsInBankRowKey = (PartnerAccountsInBankRowKey)key;
			if (partnerAccountsInBankRowKey.PartID == PartID)
			{
				return partnerAccountsInBankRowKey.SettlAccount == SettlAccount;
			}
			return false;
		}
	}

	public class Param
	{
		public string PartnerCode { get; set; }
	}

	public override string Name => "PartnerAccountsInBank";

	public PartnerAccountsInBankDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
