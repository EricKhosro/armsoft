namespace ArmSoft.OnlineBanking.Common.Enums;

public enum CardStatus : byte
{
	NotSet,
	Active,
	BlockedByUser,
	BlockedByBank,
	Expired,
	MustBeActivatedWithPin,
	MustBeActivatedWithoutPin
}
