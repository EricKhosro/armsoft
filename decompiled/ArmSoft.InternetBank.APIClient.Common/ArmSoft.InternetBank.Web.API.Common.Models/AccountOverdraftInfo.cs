using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AccountOverdraftInfo
{
	public DateTime? RepaymentDate { get; set; }

	public DateTime? SigningDate { get; set; }

	public decimal ContractAmount { get; set; }

	public decimal UsedAmount { get; set; }

	public DateTime? DebtDate { get; set; }

	public decimal DebtAmount { get; set; }

	public decimal DebtInterestAmount { get; set; }
}
