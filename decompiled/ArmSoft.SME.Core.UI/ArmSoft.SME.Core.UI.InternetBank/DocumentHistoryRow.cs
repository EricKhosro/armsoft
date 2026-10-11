using System;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class DocumentHistoryRow
{
	public Guid Id { get; set; }

	public DateTime OperationDate { get; set; }

	public short OperationType { get; set; }

	public string DisplayOperationType { get; set; }

	public string UserName { get; set; }

	public short SystemType { get; set; }

	public string DisplaySystemType { get; set; }

	public string HostAddress { get; set; }

	public string Location { get; set; }

	public string DeviceModel { get; set; }

	public string DevicePlatform { get; set; }

	public string ClientVersion { get; set; }
}
