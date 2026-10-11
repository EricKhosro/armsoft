using System;

namespace ArmSoft.SME.Common.Models.InternetBank;

public class ImportedIBTransactionsData
{
	public int Id { get; set; }

	public DateTime Date { get; set; }

	public string Account { get; set; }

	public decimal Sum { get; set; }

	public decimal SumCur { get; set; }

	public string CorrespondentAccount { get; set; }

	public bool IsCredit { get; set; }

	public string Comment { get; set; }

	public string BankDocId { get; set; }

	public Guid CreatedDocISN { get; set; }

	public short CreatedDocType { get; set; }

	public byte CreatedDocState { get; set; }

	public DateOnly CreatedDocDate { get; set; }

	public string CreatedDocNum { get; set; }

	public decimal CreatedDocSum { get; set; }

	public string CreatedDocSpecData { get; set; }
}
