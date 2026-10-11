using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class SystemVerification
{
	public string Code { get; set; } = string.Empty;

	public SystemVerificationPurposes Purpose { get; set; }

	public VerificationProcessType ProcessType { get; set; }
}
