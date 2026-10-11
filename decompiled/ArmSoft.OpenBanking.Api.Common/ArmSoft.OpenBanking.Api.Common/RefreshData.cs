namespace ArmSoft.OpenBanking.Api.Common;

public class RefreshData
{
	public string AccessToken { get; set; } = string.Empty;

	public string? RefreshToken { get; set; }
}
