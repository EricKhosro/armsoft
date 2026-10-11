namespace ArmSoft.OnlineBanking.Common.Enums;

public enum Period : byte
{
	Custom = 0,
	Today = 1,
	Yesterday = 2,
	CurrentMonth = 3,
	PreviousMonth = 4,
	CurrentYear = 8,
	FromOpeningDate = 9,
	Last3Days = 11,
	Last7Days = 12,
	Last31Days = 13,
	Last3Months = 14,
	Last6Months = 15,
	LastYear = 16
}
