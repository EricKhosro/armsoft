namespace ArmSoft.OnlineBanking.Common.Enums;

public enum MessageState : byte
{
	NotSet = 0,
	Editable = 1,
	PartiallySigned = 2,
	ReadyToBeSent = 3,
	Sent = 4,
	Confirmed = 5,
	Refused = 6,
	Accepted = 101
}
