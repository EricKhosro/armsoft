using System;

namespace ArmSoft.SME.Common.Models.Authentication;

public class AuthenticateMobileRequest
{
	public string IdToken { get; set; }

	public int DBId { get; set; }

	public string AppVersion { get; set; }

	public Guid DeviceId { get; set; }

	public bool DemoMode { get; set; }

	public long Time { get; set; }

	public int TimeZone { get; set; }
}
