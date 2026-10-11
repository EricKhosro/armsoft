namespace ArmSoft.SME.Core.UI.InternetBank;

public class IBCurrConversion : IBPayment
{
	public decimal RateValue { get; set; }

	public string CurrencyCredit { get; set; }

	public short RateFor { get; set; }

	public bool CanBeDeleted { get; set; }

	public bool CanBeSigned { get; set; }

	public string FixedCurrency { get; set; }

	public decimal AmountCredit { get; set; }

	public string RateCurrency1 { get; set; }

	public string RateCurrency2 { get; set; }

	public string RateLabel { get; set; }
}
