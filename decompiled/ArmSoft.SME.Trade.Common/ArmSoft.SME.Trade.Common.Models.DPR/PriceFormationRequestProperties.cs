using System;

namespace ArmSoft.SME.Trade.Common.Models.DPR;

public class PriceFormationRequestProperties
{
	public int MTID { get; set; }

	public string MTCode { get; set; } = string.Empty;

	public string PriceType { get; set; } = string.Empty;

	public string QntUnit { get; set; } = string.Empty;

	public DateOnly PriceDate { get; set; }

	public decimal ItemPrice { get; set; }

	public string Comment { get; set; } = string.Empty;

	public string Curr { get; set; } = string.Empty;

	public string MTCodeOld { get; set; } = string.Empty;

	public string QntUnitOld { get; set; } = string.Empty;

	public string PriceTypeOld { get; set; } = string.Empty;

	public DateOnly PriceDateOld { get; set; }
}
