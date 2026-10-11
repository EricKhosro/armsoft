using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class UserProcessActionParams
{
	public string SystemVerificationCode { get; set; } = string.Empty;

	internal Guid SystemVerificationCodeGuid
	{
		get
		{
			if (!Guid.TryParse(SystemVerificationCode, out var result))
			{
				return Guid.Empty;
			}
			return result;
		}
	}
}
