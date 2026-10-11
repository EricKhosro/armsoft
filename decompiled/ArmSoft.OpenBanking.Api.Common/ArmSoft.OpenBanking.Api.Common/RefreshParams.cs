using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class RefreshParams
{
	public string RefreshToken { get; set; } = string.Empty;

	public EasyLoginMode LoginMode { get; set; }
}
