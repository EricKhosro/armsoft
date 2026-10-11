using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum CardAccessLevels : byte
{
	None = 0,
	View = 1,
	FullControl = View
}
