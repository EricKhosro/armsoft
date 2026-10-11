namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CardLimits
{
	public decimal? CashDailyLimit { get; set; }

	public decimal? CashDailyTransactionsCount { get; set; }

	public CardLimits()
	{
		CashDailyLimit = null;
		CashDailyTransactionsCount = null;
	}
}
