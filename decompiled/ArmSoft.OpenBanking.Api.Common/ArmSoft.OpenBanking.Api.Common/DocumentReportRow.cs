using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class DocumentReportRow
{
	public string BankId { get; set; } = string.Empty;

	public Guid Id { get; set; }

	public Guid SessionId { get; set; }

	public string? ClientId { get; set; }

	public string? UserId { get; set; }

	public string? ClientName { get; set; }

	public SystemType SystemType { get; set; }

	public string Type { get; set; } = string.Empty;

	public DateTime CreationDate { get; set; }

	public DocumentProcessingStatus ProcessingStatus { get; set; }

	public string? ProcessingErrorMessage { get; set; }
}
