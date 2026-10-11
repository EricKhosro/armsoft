using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class BankLogo
{
	public required string BankCode { get; init; }

	public required byte[] Content { get; init; }

	public required string ContentType { get; init; }

	public string? FileName { get; init; }

	public DateTime UpdatedAt { get; init; }
}
