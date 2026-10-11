using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Rate
{
	public decimal RateValue { get; set; }

	public string DisplayRateValue { get; set; }

	public short RateFor { get; set; }

	public string Currency1 { get; set; }

	public string Currency2 { get; set; }

	public string RateLabel { get; set; }

	public string AmountCurrency { get; set; }

	public List<KeyValuePair<decimal, Rate>> AppliedRateDeviationsByAmount { get; set; }

	public decimal CBRateValue { get; set; }

	public short CBRateFor { get; set; }
}
