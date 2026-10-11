namespace ArmSoft.OpenBanking.Api.Common;

public class DepositOnlineData : BaseContractOnlineData
{
	public required string DepositReceiveAccount { get; set; }

	public required string PercentAmountReceiveAccount { get; set; }

	public decimal MaxWithdrawablePercent { get; set; }

	public bool IsWithdrawable { get; set; }

	public bool IsTerminatable { get; set; }
}
