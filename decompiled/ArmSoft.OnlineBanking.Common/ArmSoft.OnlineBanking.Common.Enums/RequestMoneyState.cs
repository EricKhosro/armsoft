namespace ArmSoft.OnlineBanking.Common.Enums;

public enum RequestMoneyState : byte
{
	NotSet,
	New,
	Canceled,
	Rejected,
	Confirmed
}
