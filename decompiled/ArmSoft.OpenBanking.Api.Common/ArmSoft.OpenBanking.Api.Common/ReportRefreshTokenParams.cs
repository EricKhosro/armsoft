using System.ComponentModel.DataAnnotations;

namespace ArmSoft.OpenBanking.Api.Common;

public class ReportRefreshTokenParams
{
	[Required]
	public required string RefreshToken { get; set; }
}
