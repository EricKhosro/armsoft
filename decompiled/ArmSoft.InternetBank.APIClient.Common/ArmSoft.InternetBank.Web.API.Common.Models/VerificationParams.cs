using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class VerificationParams
{
	public string Username { get; set; }

	public string UserVerificationCode { get; set; }

	public Guid? SystemVerificationCode { get; set; }

	public UserVerificationType VerificationType { get; set; }

	public bool IsForManualPasswordReset { get; set; }
}
