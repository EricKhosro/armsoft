using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum MustVerifyStatus : byte
{
	Verified = 0,
	VerifyPassword = 1,
	VerifySecurityProvider = 2
}
