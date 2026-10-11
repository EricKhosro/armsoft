namespace ArmSoft.OpenBanking.Api.Common;

public class CreateBankOperatorResponse
{
	public required BankOperator User { get; set; }

	public required string TemporaryPassword { get; set; }
}
