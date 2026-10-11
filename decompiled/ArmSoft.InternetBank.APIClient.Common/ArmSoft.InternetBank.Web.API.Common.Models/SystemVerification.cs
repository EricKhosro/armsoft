using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class SystemVerification
{
	public Guid Code { get; set; }

	public SystemVerificationPurposes Purpose { get; set; }
}
