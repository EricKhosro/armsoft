using System.Collections.Generic;

namespace ArmSoft.OpenBanking.Api.Common;

public class BankInfo
{
	public required string Id { get; set; }

	public required string Name { get; set; }

	public string? IconUrl { get; set; }

	public BankStatus Status { get; set; }

	public required List<string> AvailableModules { get; set; }
}
