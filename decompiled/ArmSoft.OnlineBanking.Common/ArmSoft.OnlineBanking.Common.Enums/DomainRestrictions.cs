using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum DomainRestrictions
{
	None = 0,
	Clients = 1,
	Operators = 2,
	All = Clients | Operators
}
