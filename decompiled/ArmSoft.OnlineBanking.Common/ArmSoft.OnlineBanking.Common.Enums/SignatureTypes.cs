using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum SignatureTypes : byte
{
	NotSet = 0,
	First = 1,
	Second = 2,
	Third = 4
}
