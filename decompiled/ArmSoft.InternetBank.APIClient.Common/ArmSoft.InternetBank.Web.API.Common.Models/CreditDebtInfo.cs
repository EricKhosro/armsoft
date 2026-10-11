namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CreditDebtInfo
{
	public CreditAmountItem Principal { get; set; }

	public CreditAmountItem Interest { get; set; }

	public CreditAmountItem Subsidy { get; set; }

	public CreditAmountItem UnusedPartsInterest { get; set; }

	public CreditAmountItem Fee { get; set; }

	public CreditAmountItem CurrencyFluctuationCharge { get; set; }

	public CreditAmountItem OverduePrincipal { get; set; }

	public CreditAmountItem OverdueInterest { get; set; }

	public CreditAmountItem OverdueFee { get; set; }

	public CreditAmountItem OverdueSubsidy { get; set; }

	public CreditAmountItem OverdueUnusedPartsInterest { get; set; }

	public CreditAmountItem PrincipalFine { get; set; }

	public CreditAmountItem InterestFine { get; set; }

	public CreditAmountItem BankReferenceRate { get; set; }

	public CreditAmountItem UnusedPartsInterestFine { get; set; }

	public CreditAmountItem PastdueSumInterest { get; set; }

	public CreditAmountItem OtherDebt { get; set; }

	public CreditAmountItem PrematureRepaymentFee { get; set; }
}
