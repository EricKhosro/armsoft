using System;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Other;

public class PartnerContractInfo
{
	public int PartID { get; set; }

	public string PartCode { get; set; }

	public string PartCaption { get; set; }

	public string PartTaxCode { get; set; }

	public string PartPhone { get; set; }

	public string PartGroup { get; set; }

	public string PartGroupName { get; set; }

	public bool PartIsCustomer { get; set; }

	public bool PartIsSupplier { get; set; }

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

	public PartnerContractInfo()
	{
	}

	public PartnerContractInfo(PartnerCacheRow partnerCacheRow, PartnerCacheRow.ContractRow contractRow)
	{
		PartID = partnerCacheRow.Id;
		PartCode = partnerCacheRow.PartCode;
		PartCaption = partnerCacheRow.Caption;
		PartTaxCode = partnerCacheRow.TaxCode;
		PartPhone = partnerCacheRow.Phone;
		PartGroup = partnerCacheRow.GroupRow?.Code ?? "";
		PartGroupName = partnerCacheRow.GroupRow?.Caption ?? "";
		PartIsCustomer = partnerCacheRow.Customer;
		PartIsSupplier = partnerCacheRow.Supplier;
		Contract = contractRow.Contract;
		Caption = contractRow.Caption;
		CurrCode = contractRow.CurrCode;
		PriceType = contractRow.PriceType;
		Limit = contractRow.Limit;
		Percent = contractRow.Percent;
		DiscountCard = contractRow.DiscountCard;
		IsBonusCard = contractRow.IsBonusCard;
		Bonus = contractRow.Bonus;
		Point = contractRow.Point;
		ControlReceivables = contractRow.ControlReceivables;
		ReceivablesLimit = contractRow.ReceivablesLimit;
		IsDefault = contractRow.IsDefault;
		ContractDate = contractRow.ContractDate;
		Description = contractRow.Description;
		BonusCardValidPeriodStart = contractRow.BonusCardValidPeriodStart;
		BonusCardValidPeriodEnd = contractRow.BonusCardValidPeriodEnd;
		BonusCardCanceled = contractRow.BonusCardCanceled;
		ExportedToDiscountServer = contractRow.ExportedToDiscountServer;
	}
}
