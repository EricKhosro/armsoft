using System;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(2)]
public class CashIn : TradeDocumentPrintWrapper
{
	[PrintDataMember("ECRCheck")]
	public ECRCheckInfo<CashRow> ECRCheck { get; set; }

	[PrintDataMember("Formula")]
	public FormulaEngine Formulas { get; set; }

	[PrintDataMember("Payer")]
	public string Payer { get; set; }

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("Base")]
	public string Base { get; set; }

	[PrintDataMember("Appendix")]
	public string Appendix { get; set; }

	[PrintDataMember("CashOutDesk")]
	public string CashOut { get; set; }

	[PrintDataMember("CashInDesk")]
	public string CashInn { get; set; }

	[PrintDataMember("Employee")]
	public string Employee { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("Time")]
	public string Time { get; set; }

	[PrintDataMember("DateInWords")]
	public string DocDateToSymbol { get; set; }

	[PrintDataMember("Comment")]
	public string Comment { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("User")]
	public UserInfo User { get; set; }

	[PrintDataMember("SummAMD")]
	public decimal SummAMD { get; set; }

	[PrintDataMember("SummCur")]
	public decimal SummCUR { get; set; }

	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

	[PrintDataMember("Partner")]
	public PartnerInfo Partner { get; set; }

	[PrintDataMember("MatRepOperationType")]
	public string OpType { get; set; }

	[PrintDataMember("CurrencyName")]
	public string CurrencyName { get; set; }

	[PrintDataMember("CurrencyCode")]
	public string CurrencyCode { get; set; }

	[PrintDataMember("SummaInWordsCur")]
	public string SumInWordsCur { get; set; }

	[PrintDataMember("SummaInWordsAMD")]
	public string SumInWordsAMD { get; set; }

	[PrintDataMember("PartnerRem")]
	public decimal PartnerDebt { get; set; }
}
