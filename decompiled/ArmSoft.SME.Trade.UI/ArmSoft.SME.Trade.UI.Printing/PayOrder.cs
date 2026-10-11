using System;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(1)]
public class PayOrder : TradeDocumentPrintWrapper
{
	[PrintDataMember("PayerName")]
	public string PayerName { get; set; }

	[PrintDataMember("PayerSettlAcc")]
	public string PayerSettlAcc { get; set; }

	[PrintDataMember("PayerBankCode")]
	public string PayerBankCode { get; set; }

	[PrintDataMember("PayerBankName")]
	public string PayerBankName { get; set; }

	[PrintDataMember("PayerTaxCode")]
	public string PayerTaxCode { get; set; }

	[PrintDataMember("ReceiverName")]
	public string ReceiverName { get; set; }

	[PrintDataMember("ReceiverFullName")]
	public string ReceiverFullName { get; set; }

	[PrintDataMember("ReceiverBankCode")]
	public string ReceiverBankCode { get; set; }

	[PrintDataMember("ReceiverBankName")]
	public string ReceiverBankName { get; set; }

	[PrintDataMember("ReceiverSettlAcc")]
	public string ReceiverSettlAcc { get; set; }

	[PrintDataMember("ReceiverTaxCode")]
	public string ReceiverTaxCode { get; set; }

	[PrintDataMember("PaymentAim")]
	public string PaymentAim { get; set; }

	[PrintDataMember("SummAMD")]
	public decimal SummAMD { get; set; }

	[PrintDataMember("SummCur")]
	public decimal SummCUR { get; set; }

	[PrintDataMember("CurrencyName")]
	public string CurrencyName { get; set; }

	[PrintDataMember("CurrencyCode")]
	public string CurrencyCode { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("DateInWords")]
	public string DocDateToSymbol { get; set; }

	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("SummaInWords")]
	public string SumInWords { get; set; }

	[PrintDataMember("SummaInWordsAMD")]
	public string SumInWordsAMD { get; set; }
}
