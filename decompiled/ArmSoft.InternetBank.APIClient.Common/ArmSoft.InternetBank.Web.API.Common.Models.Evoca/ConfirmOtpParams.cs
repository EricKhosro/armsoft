using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class ConfirmOtpParams
{
	public Guid Id { get; set; }

	public string OTP { get; set; }
}
