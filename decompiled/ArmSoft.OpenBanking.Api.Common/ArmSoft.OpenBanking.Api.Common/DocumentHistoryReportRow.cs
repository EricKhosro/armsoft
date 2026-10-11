using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class DocumentHistoryReportRow
{
	public string BankId { get; set; } = string.Empty;

	public Guid DocumentId { get; set; }

	public Guid SessionId { get; set; }

	public string? ClientId { get; set; }

	public string? UserId { get; set; }

	public string? ClientName { get; set; }

	public SystemType SystemType { get; set; }

	public string OperationType { get; set; } = string.Empty;

	public DateTime OperationDate { get; set; }

	public string IpAddress { get; set; } = string.Empty;

	public string ClientVersion { get; set; } = string.Empty;

	public string ServerVersion { get; set; } = string.Empty;

	public string? ErrorMessage { get; set; }
}
