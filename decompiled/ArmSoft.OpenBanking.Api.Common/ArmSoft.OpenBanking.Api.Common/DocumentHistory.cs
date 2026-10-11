using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class DocumentHistory
{
	public Guid Id { get; set; }

	public DateTime OperationDate { get; set; }

	public DocumentOperationType OperationType { get; set; }

	public string DisplayOperationType { get; set; } = string.Empty;

	public string UserName { get; set; } = string.Empty;

	public SystemType SystemType { get; set; }

	public string DisplaySystemType { get; set; } = string.Empty;

	public string HostAddress { get; set; } = string.Empty;

	public string Location { get; set; } = string.Empty;

	public string DeviceModel { get; set; } = string.Empty;

	public string DevicePlatform { get; set; } = string.Empty;

	public string ClientVersion { get; set; } = string.Empty;
}
