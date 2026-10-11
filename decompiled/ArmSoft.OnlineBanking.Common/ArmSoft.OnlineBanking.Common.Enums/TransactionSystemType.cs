namespace ArmSoft.OnlineBanking.Common.Enums;

public enum TransactionSystemType : byte
{
	NotSet = 0,
	WebSite = 1,
	MobileApplication = 2,
	All = byte.MaxValue
}
