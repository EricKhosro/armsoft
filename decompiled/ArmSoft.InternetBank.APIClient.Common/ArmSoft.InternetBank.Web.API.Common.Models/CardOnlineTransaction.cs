using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CardOnlineTransaction
{
	public DateTime TransactionDate { get; set; }

	public decimal Amount { get; set; }

	public string Description { get; set; }

	public string Currency { get; set; }

	public string OperationType { get; set; }

	public CardOnlineTransaction()
	{
		TransactionDate = DateTime.MinValue;
		Amount = 0m;
		Description = string.Empty;
		Currency = string.Empty;
		OperationType = string.Empty;
	}
}
