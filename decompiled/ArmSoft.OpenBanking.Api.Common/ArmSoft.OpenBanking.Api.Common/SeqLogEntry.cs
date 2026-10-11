using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class SeqLogEntry
{
	public string Id { get; set; } = string.Empty;

	public DateTime Timestamp { get; set; }

	public string Level { get; set; } = string.Empty;

	public string Message { get; set; } = string.Empty;

	public string BankId { get; set; } = string.Empty;

	public string ClientId { get; set; } = string.Empty;

	public string MachineName { get; set; } = string.Empty;

	public string ServiceName { get; set; } = string.Empty;

	public string ServiceVersion { get; set; } = string.Empty;

	public string SessionId { get; set; } = string.Empty;
}
