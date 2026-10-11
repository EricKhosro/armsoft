using System;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CardTransaction
{
	public CardTransactionType TransactionType { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime PostingDate { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime? TransactionDate { get; set; }

	public decimal BillingAmount { get; set; }

	public decimal TransactionAmount { get; set; }

	public string Description { get; set; }

	public string Currency { get; set; }

	public string TransactionCurrency { get; set; }

	public string CorrespondentAccount { get; set; }

	public string CorrespondentName { get; set; }

	public Guid? DocumentId { get; set; }

	public string OperationType { get; set; }
}
