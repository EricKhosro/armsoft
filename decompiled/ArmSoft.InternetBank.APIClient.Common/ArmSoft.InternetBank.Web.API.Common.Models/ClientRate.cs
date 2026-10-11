namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ClientRate
{
	public decimal OfferedRateValue { get; set; }

	public short RateFor { get; set; }

	public string Currency1 { get; set; }

	public string Currency2 { get; set; }

	public string RateLabel { get; set; }

	public string AmountCurrency { get; set; }

	public decimal BankRateValue { get; set; }

	public string RateDeviationMessage { get; set; }
}
