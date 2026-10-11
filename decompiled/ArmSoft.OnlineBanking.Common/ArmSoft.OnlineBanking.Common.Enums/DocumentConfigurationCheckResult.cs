using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum DocumentConfigurationCheckResult : byte
{
	Valid = 0,
	RequiredEmail = 1,
	RequiredPhoneNumber = 2,
	RequiredMobileVersionUpdate = 4
}
