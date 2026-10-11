using System;

namespace ArmSoft.Cloud.Client.AS7X;

public class OnlineLicenseInstance
{
	public int ProcessId { get; set; }

	public string ServerName { get; set; }

	public string DBName { get; set; }

	public string User { get; set; }

	public string Version { get; set; }

	public DateTime Renewed { get; set; }
}
