using System;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

public class PartnerContract
{
	public string Code { get; set; }

	public string CurrencyCode { get; set; }

	public string Name { get; set; }

	public string PriceType { get; set; }

	public decimal Percent { get; set; }

	public bool IsDefault { get; set; }

	public string DiscountCard { get; set; }

	public bool IsBonusCard { get; set; }

	public decimal Bonus { get; set; }

	public decimal Point { get; set; }

	public bool ControlReceivables { get; set; }

	public decimal ReceivablesLimit { get; set; }

	public DateTime? ContractDate { get; set; }

	public string Description { get; set; }

	public DateTime? BonusCardValidPeriodStart { get; set; }

	public DateTime? BonusCardValidPeriodEnd { get; set; }

	public bool BonusCardCanceled { get; set; }
}
