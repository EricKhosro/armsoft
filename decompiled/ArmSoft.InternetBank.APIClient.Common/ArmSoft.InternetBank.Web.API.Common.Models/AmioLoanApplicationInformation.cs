namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AmioLoanApplicationInformation
{
	public decimal LoanMaxAmount { get; set; }

	public string Currency { get; set; }

	public double LoanMinAnnualPercent { get; set; }

	public short LoanMaxTerm { get; set; }
}
