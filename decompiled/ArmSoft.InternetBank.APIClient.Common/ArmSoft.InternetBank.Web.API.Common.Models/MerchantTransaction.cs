using System;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class MerchantTransaction
{
	[JsonConverter(typeof(DateConverter))]
	public DateTime? OperationDate { get; set; }

	public string TransactionDate { get; set; }

	public string TransactionDirection { get; set; }

	public string Currency { get; set; }

	public decimal Amount { get; set; }

	public decimal MerchantFee { get; set; }

	public decimal AmountWithoutMerchantFee { get; set; }

	public string MerchantName { get; set; }

	public string CardNumber { get; set; }

	public string CardType { get; set; }

	public string CardTypeName { get; set; }

	public string CardBelongingName { get; set; }

	public string TransactionReferenceNumber { get; set; }

	public string TerminalId { get; set; }

	public string AuthorizationCode { get; set; }

	public string TokenPan { get; set; }
}
