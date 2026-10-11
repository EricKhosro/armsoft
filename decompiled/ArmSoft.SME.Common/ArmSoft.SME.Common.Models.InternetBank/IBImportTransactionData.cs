using System;

namespace ArmSoft.SME.Common.Models.InternetBank;

public class IBImportTransactionData
{
	public DateTime OperationDate { get; set; }

	public string Account { get; set; }

	public string CorrespondentAccount { get; set; }

	public string CorrespondentName { get; set; }

	public string Comment { get; set; }

	public string TransactionID { get; set; }

	public string DocumentNumber { get; set; }

	public Guid? DocumentID { get; set; }

	public decimal DebitAmount { get; set; }

	public decimal DebitAmountCur { get; set; }

	public decimal CreditAmount { get; set; }

	public decimal CreditAmountCur { get; set; }
}
