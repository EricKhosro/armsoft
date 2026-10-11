using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum DocumentCategories : byte
{
	NotSet = 0,
	Document = 1,
	Application = 2,
	Payment = 4,
	AccountPayment = 8,
	AttachedCardPayment = 0x10,
	CancellationRequest = 0x20
}
