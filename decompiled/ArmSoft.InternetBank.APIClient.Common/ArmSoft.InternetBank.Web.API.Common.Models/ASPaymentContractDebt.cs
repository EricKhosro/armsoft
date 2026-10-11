using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ASPaymentContractDebt
{
	public string Name { get; set; }

	public decimal Amount { get; set; }

	public decimal NearFee { get; set; }

	public decimal NearInterest { get; set; }

	public decimal NearPrincipal { get; set; }

	public DateTime? NearRepayDate { get; set; }

	public decimal OverdueFee { get; set; }

	public decimal OverduePrincipal { get; set; }

	public decimal OverdueInterest { get; set; }

	public decimal PenaltyAmount { get; set; }

	public decimal? PrincipalRemainder { get; set; }
}
