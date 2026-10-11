namespace ArmSoft.OpenBanking.Api.Common;

public static class ReportPasswordPolicyConstants
{
	public const int MinLength = 8;

	public const int MaxLength = 200;

	public const bool RequireUppercase = true;

	public const bool RequireLowercase = true;

	public const bool RequireDigit = true;

	public const bool MustDifferFromCurrentPassword = true;
}
