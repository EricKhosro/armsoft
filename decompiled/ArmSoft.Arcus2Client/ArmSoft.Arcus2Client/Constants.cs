namespace ArmSoft.Arcus2Client;

public static class Constants
{
	internal enum Arcus2OperationType
	{
		Purchase = 1,
		VoidOfTheLastTransaction = 2,
		Refund = 3,
		Void = 4,
		PrePurchase = 5,
		PrePurchaseCompletion = 6,
		FullLog = 7,
		ShortLog = 8,
		CloseBatch = 10,
		AdminMenu = 99
	}

	internal enum Arcus2InternalException
	{
		InitializationException,
		SetException,
		GetException
	}

	public enum Arcus2ResponseCode
	{
		Accepted1 = 0,
		Accepted2 = 1,
		Accepted3 = 3,
		Accepted4 = 20,
		SystemError = 5,
		ReconcileError = 95,
		TransactionIsNotHonored = 100,
		ExpiredCard = 101,
		CallIssuer1 = 103,
		CardIsRestricted = 104,
		CallIssuer2 = 107,
		InvalidMerchantID = 109,
		CannotProcessAmount = 110,
		InvalidAccount = 111,
		IncorrectPin = 117,
		TransactionIsNotPermitted = 119,
		IsNotPermitted = 120,
		LimitReached = 123,
		SystemMalfunction = 909,
		UnknownTransaction = 911,
		Timeout = 912,
		DuplicateTransaction = 913,
		LostOriginalTransaction = 914,
		AmountIsNotCorrect = 915,
		BadCard = 125,
		RequestInProgress = 923,
		PickUpCard = 940,
		ForcedPostNoAccount = 118,
		InsufficientFunds = 116,
		PinProcessingError1 = 920,
		PinProcessingError2 = 126,
		PinProcessingError3 = 127,
		PinProcessingError4 = 128,
		InvalidCard1 = 200,
		InvalidCard2 = 202,
		CardExpired = 201,
		CallSecurity1 = 203,
		CallSecurity2 = 205,
		AccountRestricted = 204,
		InvalidPin = 206,
		LostCard = 208,
		StolenCard = 209,
		InvalidTransaction = 902,
		TryAgain = 903,
		WrongMessage = 904,
		IssuerIsUnknown1 = 905,
		IssuerIsUnknown2 = 908,
		IssuerIsInoperable1 = 907,
		IssuerIsInoperable2 = 910,
		WithdrawalLimitExceeded = 121
	}

	public const int IgnorePrintMinErrorCode = 991;

	public const int IgnorePrintMaxErrorCode = 999;

	internal const int Arcus2GetBufferMaxSize = 80;

	internal const int Arcus2Success = 0;
}
