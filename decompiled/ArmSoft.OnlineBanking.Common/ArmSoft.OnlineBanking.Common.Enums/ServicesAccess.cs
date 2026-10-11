using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum ServicesAccess : byte
{
	NotSet = 0,
	Main = 1,
	Corporate = 2
}
