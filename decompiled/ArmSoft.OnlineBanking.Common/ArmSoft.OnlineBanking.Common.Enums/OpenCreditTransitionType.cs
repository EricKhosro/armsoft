namespace ArmSoft.OnlineBanking.Common.Enums;

public enum OpenCreditTransitionType : byte
{
	NotSet,
	InProcessBank,
	SetConditions,
	ActualPercentCheck,
	Sign,
	Refused,
	Confirmed,
	SetSubjectiveScore
}
