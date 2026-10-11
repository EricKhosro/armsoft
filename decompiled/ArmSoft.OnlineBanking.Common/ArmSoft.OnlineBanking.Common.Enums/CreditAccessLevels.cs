using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum CreditAccessLevels : byte
{
	None = 0,
	View = 1,
	RePay = 2,
	FullControl = View | RePay
}
