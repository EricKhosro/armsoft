using System;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("9D03B29B-FE58-448A-AA09-3878342C7193")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class Contract : ExchangeBase
{
	public string Code { get; set; } = string.Empty;

	public string PartnerCode { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public string PriceType { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public decimal? Percent { get; set; }

	public bool? IsDefault { get; set; }

	public string DiscountCard { get; set; } = string.Empty;

	public bool? IsBonusCard { get; set; }

	public decimal? Bonus { get; set; }

	public decimal? Point { get; set; }

	public bool? ControlReceivables { get; set; }

	public decimal? ReceivablesLimit { get; set; }

	public DateTime? ContractDate { get; set; }

	public string Description { get; set; } = string.Empty;

	public DateTime? BonusCardValidPeriodStart { get; set; }

	public DateTime? BonusCardValidPeriodEnd { get; set; }

	public bool? BonusCardCanceled { get; set; }
}
