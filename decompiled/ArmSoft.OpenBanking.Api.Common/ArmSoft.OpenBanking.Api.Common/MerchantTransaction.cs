using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class MerchantTransaction
{
	public DateTime? OperationDate { get; set; }

	public string TransactionDate { get; set; } = string.Empty;

	public string TransactionDirection { get; set; } = string.Empty;

	public string Currency { get; set; } = string.Empty;

	public decimal Amount { get; set; }

	public decimal MerchantFee { get; set; }

	public decimal AmountWithoutMerchantFee { get; set; }

	public string MerchantName { get; set; } = string.Empty;

	public string CardNumber { get; set; } = string.Empty;

	public string CardType { get; set; } = string.Empty;

	public string CardTypeName { get; set; } = string.Empty;

	public string CardBelongingName { get; set; } = string.Empty;

	public string TransactionReferenceNumber { get; set; } = string.Empty;

	public string TerminalId { get; set; } = string.Empty;

	public string AuthorizationCode { get; set; } = string.Empty;

	public string TokenPan { get; set; } = string.Empty;
}
