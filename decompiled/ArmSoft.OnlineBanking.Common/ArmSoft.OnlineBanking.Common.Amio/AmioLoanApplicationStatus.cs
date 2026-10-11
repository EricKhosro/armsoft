namespace ArmSoft.OnlineBanking.Common.Amio;

public enum AmioLoanApplicationStatus
{
	New,
	Pending,
	SetConditions,
	ActualPercentCheck,
	Sign,
	InProcess,
	Confirmed,
	Rejected,
	ClientCanceled
}
