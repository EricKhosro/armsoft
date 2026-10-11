using System;

namespace ArmSoft.OpenBanking.Api.Common;

public abstract class BaseContract
{
	public int Id { get; set; }

	public required string ContractNumber { get; set; }

	public required string Currency { get; set; }

	public required string Name { get; set; }

	public DateTime OriginalDate { get; set; }

	public decimal OriginalPrincipal { get; set; }

	public DateTime? ClosingDate { get; set; }

	public string Note { get; set; } = string.Empty;
}
