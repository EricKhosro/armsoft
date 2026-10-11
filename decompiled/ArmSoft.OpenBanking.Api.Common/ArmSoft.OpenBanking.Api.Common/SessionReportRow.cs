using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class SessionReportRow
{
	public Guid Id { get; set; }

	public string? BankId { get; set; }

	public string ClientId { get; set; } = string.Empty;

	public string UserId { get; set; } = string.Empty;

	public string? ClientName { get; set; }

	public SessionStatus Status { get; set; }

	public byte SystemType { get; set; }

	public DateTime? RefreshTokenExpiresAt { get; set; }
}
