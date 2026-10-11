using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class Partner : TradeDirectory
{
	public class PartnerContractRow
	{
		public byte[] TS = new byte[8];

		public string Contract { get; set; }

		public string Caption { get; set; }

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
	}

	public class PartnerSettlementAccount
	{
		public string SettlAccount { get; set; }

		public string Caption { get; set; }

		public bool IsDefault { get; set; }
	}

	public class PartnerBusinessAddressRow
	{
		public string Caption { get; set; }

		public bool IsDefault { get; set; }
	}

	public string FullCaption { get; set; }

	public string Type { get; set; }

	public string TaxCode { get; set; }

	public string Address { get; set; }

	public string BusinessAddress { get; set; }

	public string Phone { get; set; }

	public string Email { get; set; }

	public string ManagerFIO { get; set; }

	public string ManagerPost { get; set; }

	public string ChiefaccFIO { get; set; }

	public string ChiefaccPost { get; set; }

	public string PartStateRegister { get; set; }

	public string Passport { get; set; }

	public string IDDocType { get; set; }

	public DateOnly? Birthday { get; set; }

	public string Sex { get; set; }

	public string MainAIM { get; set; }

	public string NextContractNumber { get; set; }

	public bool Supplier { get; set; }

	public bool Customer { get; set; }

	public bool Foreign { get; set; }

	public byte B2BState { get; set; }

	public byte B2BStatusCode { get; set; }

	public int B2BUserCount { get; set; }

	public int B2BProductCount { get; set; }

	public string DefaultSettlementAccount { get; set; }

	public string DefaultContractCode { get; set; }

	public List<PartnerContractRow> PartnerContracts { get; set; } = new List<PartnerContractRow>();

	public List<PartnerContractRow> OldContracts { get; set; } = new List<PartnerContractRow>();

	public List<PartnerSettlementAccount> SettlementAccounts { get; set; } = new List<PartnerSettlementAccount>();

	public List<PartnerBusinessAddressRow> PartnerBusinessAddresses { get; set; } = new List<PartnerBusinessAddressRow>();
}
