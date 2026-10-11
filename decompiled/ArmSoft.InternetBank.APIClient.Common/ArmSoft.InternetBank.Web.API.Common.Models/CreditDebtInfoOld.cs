namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CreditDebtInfoOld
{
	public decimal Principal { get; set; }

	public decimal Interest { get; set; }

	public decimal Subsidy { get; set; }

	public decimal UnusedPartsInterest { get; set; }

	public decimal Fee { get; set; }

	public decimal CurrencyFluctuationCharge { get; set; }

	public decimal OverduePrincipal { get; set; }

	public decimal OverdueInterest { get; set; }

	public decimal OverdueFee { get; set; }

	public decimal OverdueSubsidy { get; set; }

	public decimal OverdueUnusedPartsInterest { get; set; }

	public decimal PrincipalFine { get; set; }

	public decimal InterestFine { get; set; }

	public decimal BankReferenceRate { get; set; }

	public decimal UnusedPartsInterestFine { get; set; }

	public decimal PastdueSumInterest { get; set; }

	public decimal OtherDebt { get; set; }

	public decimal PrematureRepaymentFee { get; set; }
}
