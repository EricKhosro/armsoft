namespace ArmSoft.OnlineBanking.Common.Constants;

public static class ArCaP2PResponseCode
{
	public const string Success = "0000";

	public const string TransactionFailed = "0001";

	public const string Unauthorized = "0003";

	public const string LateRequest = "0005";

	public const string NotMemberBank = "0010";

	public const string NotFoundRecipientByGivenPhoneNumber = "0011";

	public const string InvalidData = "0012";

	public const string MissingData = "0013";

	public const string InvalidRequest = "0014";

	public const string NotFoundTransactionByGivenRelativeNumber = "0015";

	public const string MobileAlreadyRegistered = "0020";

	public const string MobileAlreadyRegisteredByAnotherBank = "0021";

	public const string InvalidOtp = "0022";

	public const string UsedOtp = "0023";

	public const string ExpiredOTP = "0024";

	public const string AccountNotFound = "0025";

	public const string NewMobileIsAlreadyRegistered = "0026";

	public const string NoDefaultBank = "0027";

	public const string RecipientChangedPreferredBank = "0028";

	public const string IncorrectDataInJSON = "0030";

	public const string NotAllowedAmountValue = "0031";

	public const string AmountExceedsAvailableOperationalLimit = "0032";

	public const string ReceiverInstituteNoValidResponse = "0050";

	public const string ReceiverInstituteTimedOut = "0051";

	public const string ReceiverTransactionFailed = "0052";

	public const string InternalError = "0088";

	public const string ServiceUnavailable = "0099";

	public const string SVFEError = "SVFE";

	public const string UnknownError = "UnEr";

	public const string InvalidPhoneFormat = "invalid_phone_format";

	public const string PhoneNumberRequired = "phone_number_required";

	public const string BankCanNotBeSame = "bank_can_not_be_same";
}
