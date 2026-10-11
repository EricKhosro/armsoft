namespace ArmSoft.OpenBanking.Api.Common;

public class CreditRepaymentDetails : DocumentDetails<CreditRepayment>
{
	public string CreditOverpaymentTypeDisplayValue { get; set; } = string.Empty;

	public string CreditOverpaymentTypeDescription { get; set; } = string.Empty;
}
