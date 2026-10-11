using System;

namespace ArmSoft.Cloud.Client.AS7X;

public class InstanceInfo
{
	public string DotNetVersion { get; set; }

	public string BaseDirectory { get; set; }

	public string OSVersion { get; set; }

	public string Fingerprint { get; set; }

	public string ComputerName { get; set; }

	public Guid ComputerId { get; set; }

	public int TerminalId { get; set; }

	public int ProcessId { get; set; }
}
