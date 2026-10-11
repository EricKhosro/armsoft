namespace ArmSoft.OnlineBanking.Common.Constants;

public static class AccountTransactionType
{
	public const string Transfer = "TRF";

	public const string Miscellaneous = "MSC";

	public const string Repayment = "QIT";

	public const string Revaluation = "RVL";

	public const string InterestCalculation = "PRC";

	public const string InterestCapitalization = "INT";

	public const string CurrencyExchange = "CEX";

	public const string Provision = "RST";

	public const string Commission = "FEE";

	public const string CommissionWithCurrencyExchange = "FEX";

	public const string Penalty = "PEN";

	public const string PenaltyWithCurrencyExchange = "PEX";

	public const string Interest = "PER";

	public const string InterestWithCurrencyExchange = "PRX";

	public const string OverdraftRepayment = "OVD";

	public const string OverdraftRepaymentWithOtherAccount = "OVO";

	public const string OverdraftRepaymentWithCurrencyExchange = "OVX";

	public const string BalanceAdjustment = "ADJ";
}
