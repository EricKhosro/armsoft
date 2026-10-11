using System.ComponentModel.DataAnnotations;

namespace ArmSoft.OpenBanking.Api.Common;

public class ReportLoginParams
{
	[Required]
	[MaxLength(200)]
	public required string Username { get; set; }

	[Required]
	[MaxLength(200)]
	public required string Password { get; set; }
}
