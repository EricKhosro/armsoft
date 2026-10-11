namespace ArmSoft.OnlineBanking.Common;

public static class BankDocumentState
{
	public const short Editable = 1;

	public const short Signed1 = 2;

	public const short Signed2 = 3;

	public const short ReadyToBeSent = 4;

	public const short Sent = 5;

	public const short Sign1NotVerified = 6;

	public const short Sign2NotVerified = 7;

	public const short SignsVerified = 8;

	public const short Confirmed = 9;

	public const short Refused = 10;

	public const short KeysProcessed = 14;

	public const short Processing = 15;

	public const short LetterProcessed = 16;

	public const short Canceled = 17;

	public const short InitialProcess = 18;

	public const short ProcessedWithError = 19;

	public const short NotAutoProcessed = 20;

	public const short BankEditable = 101;

	public const short BankSigned = 102;

	public const short Received = 11;

	public const short BankSignVerified = 12;

	public const short BankSignNotVerified = 13;
}
