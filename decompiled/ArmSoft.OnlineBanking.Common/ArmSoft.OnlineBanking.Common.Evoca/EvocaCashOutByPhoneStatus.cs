namespace ArmSoft.OnlineBanking.Common.Evoca;

public enum EvocaCashOutByPhoneStatus : byte
{
	New,
	Ready,
	Error,
	InProcess,
	Problem,
	Success,
	Expired,
	Canceled
}
