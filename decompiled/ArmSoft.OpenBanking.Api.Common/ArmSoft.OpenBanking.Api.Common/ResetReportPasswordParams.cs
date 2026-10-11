using System.ComponentModel.DataAnnotations;

namespace ArmSoft.OpenBanking.Api.Common;

public class ResetReportPasswordParams
{
	[Required]
	[MaxLength(200)]
	public required string CurrentPassword { get; set; }

	[Required]
	[MinLength(8)]
	[MaxLength(200)]
	public required string NewPassword { get; set; }
}
