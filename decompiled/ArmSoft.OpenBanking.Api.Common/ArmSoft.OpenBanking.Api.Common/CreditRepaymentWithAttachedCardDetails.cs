namespace ArmSoft.OpenBanking.Api.Common;

public class CreditRepaymentWithAttachedCardDetails : DocumentDetails<CreditRepaymentWithAttachedCard>
{
	public string CreditOverpaymentTypeDisplayValue { get; set; } = string.Empty;

	public string CreditOverpaymentTypeDescription { get; set; } = string.Empty;
}
