using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AuthenticationParams
{
	public string Username { get; set; }

	public string Password { get; set; }

	public string SecurityProviderResponse { get; set; }

	public Guid AuthenticatorKey { get; set; }
}
