using System;

namespace ArmSoft.SME.Core.UI.InternetBank;

[Flags]
public enum AccountAccessLevels : byte
{
	None = 0,
	View = 1,
	MakeOperation = 2,
	ViewDocument = 4,
	FullControl = View | MakeOperation | ViewDocument
}
