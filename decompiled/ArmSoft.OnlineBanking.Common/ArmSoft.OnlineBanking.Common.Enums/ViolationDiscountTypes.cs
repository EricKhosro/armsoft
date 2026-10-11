using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum ViolationDiscountTypes : byte
{
	NoDiscount = 0,
	DiscountPercentIs30 = 1,
	DiscountPercentIs20 = 2,
	DiscountPercentAll = DiscountPercentIs30 | DiscountPercentIs20
}
