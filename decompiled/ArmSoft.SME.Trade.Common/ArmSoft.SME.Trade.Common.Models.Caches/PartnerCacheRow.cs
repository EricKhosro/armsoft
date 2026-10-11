using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class PartnerCacheRow : CacheRowBaseWithIdName<string>
{
	public class ContractRow
	{
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

		public byte[] TS { get; set; }

		public ContractRow(string contract, string caption, string currCode, string priceType, decimal limit, decimal percent, string discountCard, bool isBonusCard, decimal bonus, decimal point, bool controlReceivables, decimal receivablesLimit, bool isDefault, DateOnly? contractDate, string description, DateOnly? bonusCardValidPeriodStart, DateOnly? bonusCardValidPeriodEnd, bool bonusCardCanceled, bool exportedToDiscountServer, byte[] ts)
		{
			Contract = contract;
			Caption = caption;
			CurrCode = currCode;
			PriceType = priceType;
			Limit = limit;
			Percent = percent;
			DiscountCard = discountCard;
			IsBonusCard = isBonusCard;
			Bonus = bonus;
			Point = point;
			ControlReceivables = controlReceivables;
			ReceivablesLimit = receivablesLimit;
			IsDefault = isDefault;
			ContractDate = contractDate;
			Description = description;
			BonusCardValidPeriodStart = bonusCardValidPeriodStart;
			BonusCardValidPeriodEnd = bonusCardValidPeriodEnd;
			BonusCardCanceled = bonusCardCanceled;
			ExportedToDiscountServer = exportedToDiscountServer;
			TS = ts;
		}

		public object GetReferenceValue(string name)
		{
			return name switch
			{
				"ContractCacheContract" => (object)Contract, 
				"ContractCacheCaption" => Caption, 
				"ContractCacheCurrCode" => CurrCode, 
				"ContractCachePriceType" => PriceType, 
				"ContractCacheReceivablesLimit" => ReceivablesLimit, 
				"ContractCacheContractDate" => ContractDate, 
				_ => null, 
			};
		}
	}

	public class SettlementAccountRow
	{
		public string SettlAccount { get; }

		public string Caption { get; }

		public bool IsDefault { get; }

		[JsonConstructor]
		public SettlementAccountRow(string settlAccount, string caption, bool isDefault)
		{
			SettlAccount = settlAccount;
			Caption = caption;
			IsDefault = isDefault;
		}
	}

	public const string PartnerCache = "PartnerCache";

	public const string ContractCache = "ContractCache";

	private readonly Func<string, PartnerGroupCacheRow> _groupLookup;

	public string PartCode { get; set; }

	public string Caption { get; set; }

	public string FullCaption { get; set; }

	public string Type { get; set; }

	[JsonIgnore]
	public PartnerGroupCacheRow GroupRow
	{
		get
		{
			if (_groupLookup != null)
			{
				return _groupLookup(Type);
			}
			return GroupUIResolver?.Invoke(Type);
		}
	}

	[JsonIgnore]
	public static Func<string, PartnerGroupCacheRow> GroupUIResolver { get; set; }

	public bool Supplier { get; set; }

	public bool Customer { get; set; }

	public bool Foreign { get; set; }

	public string TaxCode { get; set; }

	public string SettlAcc { get; set; }

	public string IDDocType { get; set; }

	public string Passport { get; set; }

	public string Email { get; set; }

	public string PartStateRegister { get; set; }

	public string Address { get; set; }

	public string BusinessAddress { get; set; }

	public string ManagerFIO { get; set; }

	public string ManagerPost { get; set; }

	public string ChiefaccFIO { get; set; }

	public string ChiefaccPost { get; set; }

	public string MainAIM { get; set; }

	public string Phone { get; set; }

	public string NextContractNumber { get; set; }

	public DateOnly? Birthday { get; set; }

	public string Sex { get; set; }

	public byte B2BStatusCode { get; set; }

	public string B2BStatusName { get; set; }

	public List<SettlementAccountRow> SettlementAccounts { get; set; }

	public List<string> BusinessAddresses { get; set; }

	public List<ContractRow> Contracts { get; set; }

	[JsonIgnore]
	public static Func<string, string, object> ExtensionUIResolver { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(PartCode);

	public PartnerCacheRow()
	{
	}

	public PartnerCacheRow(int id, string partCode, string caption, string fullCaption, string type, bool foreign, bool supplier, bool customer, string taxCode, string idDocType, string passport, string email, string settlAcc, string address, string businessAddress, string partStateRegister, string mainAIM, string phone, string managerFIO, string managerPost, string chiefaccFIO, string chiefaccPost, string nextContractNumber, string sex, DateOnly? birthday, byte b2bStatusCode, byte[] ts, List<SettlementAccountRow> settlementAccounts, List<string> businessAddresses, List<ContractRow> contracts, Func<string, PartnerGroupCacheRow> groupLookup)
	{
		Id = id;
		PartCode = partCode;
		Caption = caption;
		FullCaption = fullCaption;
		Type = type;
		_groupLookup = groupLookup;
		Foreign = foreign;
		Supplier = supplier;
		Customer = customer;
		TaxCode = taxCode;
		Address = address;
		BusinessAddress = businessAddress;
		ManagerFIO = managerFIO;
		ManagerPost = managerPost;
		ChiefaccFIO = chiefaccFIO;
		ChiefaccPost = chiefaccPost;
		NextContractNumber = nextContractNumber;
		Sex = sex;
		Birthday = birthday;
		MainAIM = mainAIM;
		Phone = phone;
		Email = email;
		SettlAcc = settlAcc;
		PartStateRegister = partStateRegister;
		IDDocType = idDocType;
		Passport = passport;
		B2BStatusCode = b2bStatusCode;
		B2BStatusName = (SmartEnum<PartnerStatusInB2BSE, byte>.TryFromValue(b2bStatusCode, out var result) ? result.Caption : ArmSoft.SME.Common.Resources.Unknown);
		TS = ts;
		SettlementAccounts = settlementAccounts;
		BusinessAddresses = businessAddresses;
		Contracts = contracts;
	}

	public object GetReferenceValue(string name)
	{
		if (name.StartsWith("PartnerGroupCache"))
		{
			return GroupRow.GetReferenceValue(name);
		}
		return name switch
		{
			"PartnerCachePartCode" => PartCode, 
			"PartnerCacheCaption" => Caption, 
			"PartnerCacheFullCaption" => FullCaption, 
			"PartnerCacheType" => Type, 
			"PartnerCacheTaxCode" => TaxCode, 
			"PartnerCachePhone" => Phone, 
			"PartnerCacheEmail" => Email, 
			"PartnerCacheAddress" => Address, 
			"PartnerCachePassport" => Passport, 
			"PartnerCacheBusinessAddress" => BusinessAddress, 
			"PartnerCacheManagerFIO" => ManagerFIO, 
			"PartnerCacheManagerPost" => ManagerPost, 
			"PartnerCacheChiefaccFIO" => ChiefaccFIO, 
			"PartnerCacheChiefaccPost" => ChiefaccPost, 
			"PartnerCacheSettlAcc" => SettlAcc, 
			"PartnerCacheMainAIM" => MainAIM, 
			"PartnerCacheIDDocType" => IDDocType, 
			_ => ExtensionUIResolver?.Invoke(PartCode, name.Substring("PartnerCache".Length)), 
		};
	}
}
