namespace ArmSoft.OpenBanking.Api.Common;

public class ResetBankOperatorResponse
{
	public int UserId { get; set; }

	public required string TemporaryPassword { get; set; }
}
