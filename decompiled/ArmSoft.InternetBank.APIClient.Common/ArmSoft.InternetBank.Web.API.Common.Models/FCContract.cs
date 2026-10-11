using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class FCContract
{
	public string Currency { get; set; }

	public string PaymentCurrency { get; set; }

	public int Status { get; set; }

	public string ContractName { get; set; }

	public string Details { get; set; }

	public decimal? PrincipalRemainder { get; set; }

	public DateTime? DebtDate { get; set; }

	public decimal Total { get; set; }

	public CreditDebtInfo DebtInfo { get; set; }
}
