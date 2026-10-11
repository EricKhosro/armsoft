namespace ArmSoft.SME.Core.UI.InternetBank;

public class InternetBankLoginRequest
{
	public string BankCode { get; set; }

	public string User { get; set; }

	public string Password { get; set; }

	public string Challenge { get; set; }

	public string SecurityKey { get; set; }

	public bool IsOnlyDigiPass { get; set; }

	public string Description { get; set; }

	public string Token { get; set; }
}
