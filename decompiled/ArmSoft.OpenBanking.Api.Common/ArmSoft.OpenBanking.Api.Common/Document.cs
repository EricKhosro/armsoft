namespace ArmSoft.OpenBanking.Api.Common;

public class Document : SignableObject
{
	public string Number { get; set; } = string.Empty;

	public string Category { get; set; } = string.Empty;

	public string Type { get; set; } = string.Empty;

	public string Details { get; set; } = string.Empty;

	public string? TemplateName { get; set; }

	public decimal CommissionAmount { get; set; }

	public string CommissionCurrency { get; set; } = string.Empty;

	public decimal CommissionAmount2 { get; set; }

	public string CommissionCurrency2 { get; set; } = string.Empty;

	public string ChargeAccount { get; set; } = string.Empty;

	public string ChargeAccountCurrency { get; set; } = string.Empty;

	public bool IsManuallyEntered { get; set; }
}
