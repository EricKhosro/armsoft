using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ActivateSecurityProviderParams
{
	public string Username { get; set; }

	public Guid SystemVerificationCode { get; set; }

	public string SecurityProvider { get; set; }

	public string TokenSerial { get; set; }

	public bool IsUserOtpIntegratedDevice { get; set; }

	public string Uuid { get; set; }

	public ActivateSecurityProviderParams()
	{
		IsUserOtpIntegratedDevice = false;
		Uuid = null;
	}
}
