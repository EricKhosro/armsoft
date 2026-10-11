namespace ArmSoft.OnlineBanking.Common.Constants;

public static class QrPaymentResponseCode
{
	public const string Approved = "000";

	public const string SystemError = "005";

	public const string DoNotHonorTransaction = "100";

	public const string InvalidMerchantOrTerminalId = "109";

	public const string RefundTransactionSumExceeded = "110";

	public const string DeclinedTransactionCannotBeReversed = "111";

	public const string AmountExceedsAvailableOperationalLimit = "121";

	public const string InsufficientFunds = "116";

	public const string TimeoutWaitingForResponse = "912";

	public const string DuplicateTransactionReceived = "913";

	public const string CouldNotFindTheOriginalTransaction = "914";

	public const string TransactionAmountMismatch = "129";
}
