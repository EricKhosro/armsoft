using System;

namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class Contract : TradeDirectory
{
	public int PartID { get; set; }

	public string PartCode { get; set; }

	public string PartCaption { get; set; }

	public string CurrCode { get; set; }

	public string PriceType { get; set; }

	public string PriceTypeCaption { get; set; }

	public DateOnly? ContractDate { get; set; }

	public string Description { get; set; }

	public decimal DiscountPercent { get; set; }

	public bool ControlReceivables { get; set; }

	public decimal ReceivablesLimit { get; set; }

	public bool IsDefault { get; set; }

	public decimal Limit { get; set; }

	public string DiscountCard { get; set; }

	public bool IsBonusCard { get; set; }

	public decimal Bonus { get; set; }

	public decimal Point { get; set; }

	public DateOnly? BonusCardValidPeriodStart { get; set; }

	public DateOnly? BonusCardValidPeriodEnd { get; set; }

	public bool BonusCardCanceled { get; set; }

	public bool ExportedToDiscountServer { get; set; }
}
