using System;

namespace ArmSoft.OpenBanking.Api.Common;

public abstract class BaseContractOnlineData
{
	public int Id { get; set; }

	public required string ContractNumber { get; set; }

	public string Currency { get; set; }

	public DateTime MaturityDate { get; set; }

	public required string InterestRate { get; set; }

	public decimal OutstandingPrincipal { get; set; }

	public decimal CalculatedInterest { get; set; }
}
