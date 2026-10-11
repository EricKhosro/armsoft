namespace ArmSoft.OnlineBanking.Common.Enums;

public enum DocumentOperationType : byte
{
	NotSet,
	Created,
	Edited,
	Signed,
	FirstSign,
	SecondSign,
	Sent,
	Confirmed,
	Refused,
	Canceled,
	SendToEdit,
	ThirdSign,
	PartiallySigned,
	ConfirmedCommissionChange,
	SignatureQueued,
	SignatureFailed,
	SignatureIgnored
}
