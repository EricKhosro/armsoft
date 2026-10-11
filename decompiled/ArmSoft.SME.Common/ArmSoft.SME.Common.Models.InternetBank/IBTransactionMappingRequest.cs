using System;

namespace ArmSoft.SME.Common.Models.InternetBank;

public class IBTransactionMappingRequest
{
	public DateTime Date { get; set; }

	public string Account { get; set; }

	public bool IsCredit { get; set; }

	public decimal Sum { get; set; }

	public decimal SumCur { get; set; }

	public string CorrespondentAccount { get; set; }

	public string CorrespondentAccountName { get; set; }

	public string Comment { get; set; }

	public Guid MatchedDocISN { get; set; }

	public Guid DocumentID { get; set; }

	public string BankDocId { get; set; }

	public string BankDocNumber { get; set; }
}
