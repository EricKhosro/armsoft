namespace ArmSoft.OnlineBanking.Common.Evoca;

public enum EvocaMobileLoanApplicationStatus : byte
{
	New,
	Pending,
	ScoreApproved,
	Confirmed,
	ClientRejected,
	OTPApproved,
	Approved,
	Rejected,
	ReCreate
}
