namespace ArmSoft.OnlineBanking.Common.Enums;

public enum DocumentState : byte
{
	NotSet = 0,
	Editable = 1,
	PartiallySigned = 2,
	ReadyToBeSent = 3,
	Sent = 4,
	InProcessBank = 5,
	Confirmed = 6,
	Refused = 7,
	Canceled = 8,
	InProcessOnline = 9,
	Active = 10,
	PeriodicPaymentTemplate = 98,
	Template = 99,
	Ready = 100,
	Accepted = 101
}
