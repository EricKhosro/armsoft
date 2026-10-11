using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum DepositAccessLevels : byte
{
	None = 0,
	View = 1,
	Attract = 2,
	FullControl = View | Attract
}
