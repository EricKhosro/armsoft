namespace ArmSoft.OpenBanking.Api.Common;

public sealed class ReportPasswordPolicyResponse
{
	public int MinLength { get; set; }

	public int MaxLength { get; set; }

	public bool RequireUppercase { get; set; }

	public bool RequireLowercase { get; set; }

	public bool RequireDigit { get; set; }
}
