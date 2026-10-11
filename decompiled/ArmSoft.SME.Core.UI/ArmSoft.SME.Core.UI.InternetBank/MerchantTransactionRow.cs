using System;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class MerchantTransactionRow
{
	public DateOnly? OperationDate { get; set; }

	public DateTime? TransactionDate { get; set; }

	public string TransactionType { get; set; }

	public string Currency { get; set; }

	public decimal Amount { get; set; }

	public decimal MerchantFee { get; set; }

	public decimal AmountWithoutMerchantFee { get; set; }

	public string MerchantName { get; set; }

	public string CardNumber { get; set; }

	public string CardTypeName { get; set; }

	public string CardBelongingName { get; set; }

	public string TokenPan { get; set; }

	public string RRN { get; set; }

	public string TerminalId { get; set; }

	public string AuthorizationCode { get; set; }
}
