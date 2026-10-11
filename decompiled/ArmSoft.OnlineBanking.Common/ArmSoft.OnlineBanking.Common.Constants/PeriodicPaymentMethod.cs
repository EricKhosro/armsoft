namespace ArmSoft.OnlineBanking.Common.Constants;

public static class PeriodicPaymentMethod
{
	public const string DebitAccountPositiveBalance = "01";

	public const string DebitAccountNegativeBalance = "02";

	public const string CreditAccountPositiveBalance = "03";

	public const string CreditAccountNegativeBalance = "04";

	public const string MinimumOfPositiveBalances = "05";

	public const string CreditTurnoverOfDebitAccount = "06";

	public const string CreditTurnoverOfDebitAccountMinusDebitTurnover = "07";

	public const string DebitTurnoverOfDebitAccount = "08";

	public const string DebitTurnoverOfDebitAccountMinusCreditTurnover = "09";

	public const string FixedAmount = "10";

	public const string DebtAmount = "11";
}
