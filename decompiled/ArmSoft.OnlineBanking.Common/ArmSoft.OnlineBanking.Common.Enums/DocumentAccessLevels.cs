using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum DocumentAccessLevels : byte
{
	None = 0,
	View = 1,
	Modify = 2,
	Sign = 4,
	ViewAndModify = View | Modify,
	ViewAndSign = View | Sign,
	ModifyAndSign = Modify | Sign,
	FullControl = ViewAndModify | Sign
}
