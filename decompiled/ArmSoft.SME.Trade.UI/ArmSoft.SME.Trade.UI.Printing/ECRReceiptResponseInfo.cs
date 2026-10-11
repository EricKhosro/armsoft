using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class ECRReceiptResponseInfo
{
	[PrintDataMember("Address")]
	public string Address { get; set; }

	[PrintDataMember("SummaChange")]
	public double ChangeSum { get; set; }

	[PrintDataMember("ECRCrn")]
	public string ECRCRN { get; set; }

	[PrintDataMember("ECRSN")]
	public string ECRSN { get; set; }

	[PrintDataMember("FiscalNum")]
	public string FiscalNum { get; set; }

	[PrintDataMember("LotteryNum")]
	public string LotteryNum { get; set; }

	[PrintDataMember]
	public int Prize { get; set; }

	[PrintDataMember("ECRCheckNum")]
	public string ECRCheckNum { get; set; }

	[PrintDataMember("PrePaymentECRCheckNum")]
	public string PrePaymentECRCheckNum { get; set; }

	[PrintDataMember("CompanyName")]
	public string CompanyName { get; set; }

	[PrintDataMember("Time")]
	public string Time { get; set; }

	[PrintDataMember("OptionsTaxCode")]
	public string TaxCode { get; set; }

	[PrintDataMember("Total")]
	public double Total { get; set; }

	[PrintDataMember("ECROperator")]
	public CodeCaption ECROperator { get; set; }

	[PrintDataMember("MarksCount")]
	public string EMarksCount { get; set; }

	[PrintDataMember("VerificationNumber")]
	public string VerificationNumber { get; set; }
}
