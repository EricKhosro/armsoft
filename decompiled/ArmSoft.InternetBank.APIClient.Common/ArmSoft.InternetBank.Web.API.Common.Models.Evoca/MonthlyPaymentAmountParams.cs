namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class MonthlyPaymentAmountParams
{
	public string LoanType { get; set; }

	public int Term { get; set; }

	public decimal Amount { get; set; }
}
