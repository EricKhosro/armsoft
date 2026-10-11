using System;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class ContractInfo : CodeCaption
{
	[PrintDataMember("Curr")]
	public string Curr { get; set; }

	[PrintDataMember("PriceType")]
	public string PriceType { get; set; }

	[PrintDataMember("DiscountPercent")]
	public decimal DiscountPercent { get; set; }

	[PrintDataMember("DiscountCardCode")]
	public string DiscountCard { get; set; }

	[PrintDataMember("BonusCard")]
	public bool IsBonusCard { get; set; }

	[PrintDataMember("BonusPercent")]
	public decimal Bonus { get; set; }

	[PrintDataMember("Default")]
	public bool IsDefault { get; set; }

	[PrintDataMember("ContractDate")]
	public DateOnly? ContractDate { get; set; }

	[PrintDataMember("TemplatesDescription")]
	public string ContractDescr { get; set; }
}
