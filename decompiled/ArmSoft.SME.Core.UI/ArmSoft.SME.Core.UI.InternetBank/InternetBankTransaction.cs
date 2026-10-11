using System;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class InternetBankTransaction
{
	public DateTime CreationDate { get; set; }

	public DateTime OperationDate { get; set; }

	public decimal DebitAmountCur { get; set; }

	public decimal DebitAmount { get; set; }

	public decimal CreditAmountCur { get; set; }

	public decimal CreditAmount { get; set; }

	public string Currency { get; set; }

	public string OperationType { get; set; }

	public string Comment { get; set; }

	public string Account { get; set; }

	public string CorrespondentAccount { get; set; }

	public string CorrespondentName { get; set; }

	public string DocumentNumber { get; set; }

	public string TransactionID { get; set; }

	public string TransactionShortID { get; set; }

	public Guid? DocumentID { get; set; }

	public string DocumentIDString { get; set; }
}
