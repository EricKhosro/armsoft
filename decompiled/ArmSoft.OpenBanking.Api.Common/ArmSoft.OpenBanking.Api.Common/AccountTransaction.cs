using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class AccountTransaction
{
	public DateTime CreationDate { get; set; }

	public DateTime OperationDate { get; set; }

	public decimal DebitAmount { get; set; }

	public decimal DebitAmountAMD { get; set; }

	public decimal CreditAmount { get; set; }

	public decimal CreditAmountAMD { get; set; }

	public string OperationType { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public string CorrespondentAccount { get; set; } = string.Empty;

	public string CorrespondentName { get; set; } = string.Empty;

	public string DocumentNumber { get; set; } = string.Empty;

	public string TransactionId { get; set; } = string.Empty;

	public Guid? DocumentId { get; set; }

	public string Currency { get; set; } = string.Empty;
}
