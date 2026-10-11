using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum UtilitySearchCriteria : byte
{
	None = 0,
	Id = 2,
	Phone = 4
}
