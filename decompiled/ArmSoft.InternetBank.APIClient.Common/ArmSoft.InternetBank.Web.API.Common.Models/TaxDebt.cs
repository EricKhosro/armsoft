using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class TaxDebt
{
	public string Id { get; set; }

	public decimal Amount { get; set; }

	public decimal TaxAmount { get; set; }

	public string LastUpdateDate { get; set; }

	public string Info { get; set; }

	public string Year { get; set; }

	public string BankAccount { get; set; }

	public TaxDebtType TaxDebtType { get; set; }

	public string DisplayTaxDebtType { get; set; }

	public string TaxPayerName { get; set; }

	public string TaxPayerAddress { get; set; }
}
