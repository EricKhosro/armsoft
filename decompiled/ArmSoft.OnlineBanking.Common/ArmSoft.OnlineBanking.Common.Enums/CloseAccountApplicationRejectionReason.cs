namespace ArmSoft.OnlineBanking.Common.Enums;

public enum CloseAccountApplicationRejectionReason : byte
{
	NotSet,
	AccountBalanceNonZero,
	CurrencyAccountBalanceNotZero,
	UpcomingOperationsExist,
	OperationsExist,
	ClosingDateBeforeOpening,
	AccountIsUsed
}
