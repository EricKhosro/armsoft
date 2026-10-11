namespace ArmSoft.OnlineBanking.Common.Enums;

public enum CreditOverpaymentType : byte
{
	NotSet,
	UpcomingAmounts,
	LastAmounts,
	UpcomingAmountThenLast,
	WithScheduleRedistribution
}
