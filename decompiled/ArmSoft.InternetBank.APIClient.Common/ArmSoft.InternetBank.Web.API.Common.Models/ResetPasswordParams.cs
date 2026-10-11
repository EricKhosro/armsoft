using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ResetPasswordParams
{
	public Guid SystemVerificationCode { get; set; }

	public UserVerificationType VerificationType { get; set; }

	public string NewPassword { get; set; }
}
