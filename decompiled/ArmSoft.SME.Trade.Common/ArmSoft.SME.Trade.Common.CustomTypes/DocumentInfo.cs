using System;
using System.Collections.Generic;
using ArmSoft.SME.Trade.Common.Models.Documents;

namespace ArmSoft.SME.Trade.Common.CustomTypes;

public class DocumentInfo
{
	public DateOnly DocDate { get; set; }

	public string PartnerCode { get; set; }

	public string PartnerContract { get; set; }

	public decimal? PartnerContractPercent { get; set; }

	public string CurrCode { get; set; } = string.Empty;

	public decimal CurrRate { get; set; } = 1m;

	public decimal CurrBase { get; set; } = 1m;

	public string VATType { get; set; } = string.Empty;

	public string PriceType { get; set; } = string.Empty;

	public string PriceListTypeRoundDgr { get; set; } = string.Empty;

	public string SaleConsultant { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

	public string Cash { get; set; } = string.Empty;

	public decimal TotalAmount { get; set; }

	public string Comment { get; set; } = string.Empty;

	public IEnumerable<ISaleRow> Rows { get; set; }
}
