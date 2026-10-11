using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum UserVerificationOptions : byte
{
	False = 0,
	TrueForClients = 1,
	TrueForOperators = 2,
	True = TrueForClients | TrueForOperators
}
