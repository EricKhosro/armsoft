using System;

namespace ArmSoft.Cloud.Client.AS7X;

public class OnlineLicenseInfo
{
	public string Station { get; set; }

	public string IPAddress { get; set; }

	public int TerminalId { get; set; }

	public Guid LicenseId { get; set; }

	public int ProcessCount { get; set; }

	public DateTime LastRenewed { get; set; }

	public string Info { get; set; }
}
