namespace ArmSoft.OnlineBanking.Common;

public static class QrPaymentOnlineStatus
{
	public const string Pending = "--";

	public const string ProcessedAutomatically = "11";

	public const string ProcessedManually = "12";

	public const string CancelledAutomatically = "01";

	public const string CancelledManually = "02";
}
