using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class ContractsDataProvider : DataProvider<ContractsDataProvider.DataRow, ContractsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public class Key
		{
			public int PartnerId { get; set; }

			public string Contract { get; set; }
		}

		public int PartnerId { get; set; }

		public string PartnerCode { get; set; }

		public string PartnerName { get; set; }

		public string PartnerTaxCode { get; set; }

		public string PartnerPhone { get; set; }

		public string PartnerGroup { get; set; }

		public string PartnerGroupName { get; set; }

		public bool PartnerIsCustomer { get; set; }

		public bool PartnerIsSupplier { get; set; }

		public string Contract { get; set; }

		public string Name { get; set; }

		public string CurrCode { get; set; }

		public string PriceType { get; set; }

		public decimal Limit { get; set; }

		public decimal Percent { get; set; }

		public string DiscountCard { get; set; }

		public bool IsBonusCard { get; set; }

		public decimal Bonus { get; set; }

		public decimal Point { get; set; }

		public bool ControlReceivables { get; set; }

		public decimal ReceivablesLimit { get; set; }

		public bool IsDefault { get; set; }

		public DateOnly? ContractDate { get; set; }

		public string Description { get; set; }

		public DateOnly? BonusCardValidPeriodStart { get; set; }

		public DateOnly? BonusCardValidPeriodEnd { get; set; }

		public bool BonusCardCanceled { get; set; }

		public bool ExportedToDiscountServer { get; set; }

		public bool Matched(object key)
		{
			Key key2 = (Key)key;
			if (key2.PartnerId == PartnerId)
			{
				return key2.Contract == Contract;
			}
			return false;
		}

		public object GetKey()
		{
			return new Key
			{
				PartnerId = PartnerId,
				Contract = Contract
			};
		}

		public string GetCode()
		{
			return "";
		}

		public string GetName()
		{
			return Name;
		}
	}

	public class Param
	{
		public string PartnerCode { get; set; }

		public string CurrCode { get; set; }

		public bool IsExceptCurrCodeNCC { get; set; }

		public bool IsFilterBySuppliers { get; set; }

		public bool IsFilterByCustomers { get; set; }

		public bool ShowOnlyBonusCards { get; set; }
	}

	public override string Name => "Contracts";

	public ContractsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
