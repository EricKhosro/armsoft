using System;

namespace ArmSoft.OpenBanking.Api.Common;

public sealed class BankOperatorAuditLogItem
{
	public long Id { get; set; }

	public int? UserId { get; set; }

	public Guid? SessionId { get; set; }

	public ReportAuditActorType ActorType { get; set; }

	public string? ActorId { get; set; }

	public string EventType { get; set; } = string.Empty;

	public bool Success { get; set; }

	public string? IpAddress { get; set; }

	public string? Details { get; set; }

	public DateTime DateUtc { get; set; }
}
