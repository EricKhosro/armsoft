using System;

namespace ArmSoft.SME.Core.AddIn;

public class AuthenticateRequest
{
	public string IdToken { get; set; }

	public int DBId { get; set; }

	public short AppType { get; set; }

	public string AppVersion { get; set; }

	public string CompName { get; set; }

	public int ProcessId { get; set; }

	public string DotNetVersion { get; set; }

	public string BaseDirectory { get; set; }

	public string OSVersion { get; set; }

	public Guid CompId { get; set; }

	public bool DemoMode { get; set; }
}
