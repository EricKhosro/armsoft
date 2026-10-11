using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class SendVerificationCodeParams
{
	public Guid SystemVerificationCode { get; set; }

	public UserVerificationType UserVerificationType { get; set; }
}
