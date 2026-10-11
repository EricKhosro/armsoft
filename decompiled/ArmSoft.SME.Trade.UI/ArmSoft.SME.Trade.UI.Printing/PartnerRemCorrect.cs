using System;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(6)]
public class PartnerRemCorrect : TradeDocumentPrintWrapper
{
	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("Comment")]
	public string Comment { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

	[PrintDataMember("Partner")]
	public PartnerInfo Partner { get; set; }

	[PrintDataMember("Contract")]
	public string Contract { get; set; }

	[PrintDataMember("PartnerRem")]
	public decimal PartnerRem { get; set; }

	[PrintDataMember("SummAMD")]
	public decimal SummAMD { get; set; }

	[PrintDataMember("SummCur")]
	public decimal SummCUR { get; set; }

	[PrintDataMember("CurrencyName")]
	public string CurrencyName { get; set; }

	[PrintDataMember("CurrencyCode")]
	public string CurrencyCode { get; set; }

	[PrintDataMember("SummaInWordsCur")]
	public string SumInWordsCur { get; set; }

	[PrintDataMember("SummaInWordsAMD")]
	public string SumInWordsAMD { get; set; }
}
