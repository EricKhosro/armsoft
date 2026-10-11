using System;
using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(34)]
public class Reconcilation : TradeDocumentPrintWrapper
{
	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("CurrencyCode")]
	public string CurrencyCode { get; set; }

	[PrintDataMember("CurrencyName")]
	public string CurrencyName { get; set; }

	[PrintDataMember("Partner")]
	public PartnerInfo Partner { get; set; }

	[PrintDataMember("DatePeriodBegin")]
	public DateOnly? PeriodStartDate { get; set; }

	[PrintDataMember("DatePeriodEnd")]
	public DateOnly? PeriodEndDate { get; set; }

	[PrintDataMember("Comment")]
	public string Comment { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("InitRem")]
	public decimal StartRem { get; set; }

	[PrintDataMember("EndRem")]
	public decimal EndRem { get; set; }

	[PrintDataMember]
	public decimal StartDbRem { get; set; }

	[PrintDataMember]
	public decimal EndDbRem { get; set; }

	[PrintDataMember]
	public decimal StartCrRem { get; set; }

	[PrintDataMember]
	public decimal EndCrRem { get; set; }

	[PrintDataMember("OurCalculations")]
	public BindingList<ReconciliationCalcGridRow> OurCalcList { get; set; }
}
