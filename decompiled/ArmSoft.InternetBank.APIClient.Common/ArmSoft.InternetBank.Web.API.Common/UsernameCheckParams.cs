using System;

namespace ArmSoft.InternetBank.Web.API.Common;

public class UsernameCheckParams
{
	public string Username { get; set; }

	public Guid SystemVerificationCode { get; set; }
}
