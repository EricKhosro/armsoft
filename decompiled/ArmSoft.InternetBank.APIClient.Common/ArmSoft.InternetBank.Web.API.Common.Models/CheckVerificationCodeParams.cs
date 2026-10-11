using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CheckVerificationCodeParams
{
	public string UserVerificationCode { get; set; }

	public Guid? SystemVerificationCode { get; set; }

	public UserVerificationType VerificationType { get; set; }
}
