using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Client.Models;

public class TradeDocumentDataRow : DocumentsDataRow
{
	public int OrderNum { get; set; }

	public string CurrCode { get; set; }

	public decimal Summ { get; set; }
}
