using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class ECRReceiptResponseInfo
{
	public string Address { get; set; }

	public double ChangeSum { get; set; }

	public string ECRCRN { get; set; }

	public string ECRSN { get; set; }

	public string FiscalNum { get; set; }

	public string LotteryNum { get; set; }

	public int Prize { get; set; }

	public string ECRCheckNum { get; set; }

	public string PrePaymentECRCheckNum { get; set; }

	public string CompanyName { get; set; }

	public string Time { get; set; }

	public string TaxCode { get; set; }

	public double Total { get; set; }

	public CodeName ECROperator { get; set; } = new CodeName();

	public string EMarksCount { get; set; }

	public string VerificationNumber { get; set; }
}
