using System;
using System.ComponentModel;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(24)]
public class MTIncomeRem : TradeDocumentPrintWrapper
{
	[PrintDataMember("Formula")]
	public FormulaEngine Formulas { get; set; }

	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("Supplier")]
	public PartnerInfo Supplier { get; set; }

	[PrintDataMember("Curr")]
	public string Curr { get; set; }

	[PrintDataMember("InvoiceNumber")]
	public string InvoiceNumber { get; set; }

	[PrintDataMember("CreateType")]
	public string CreateType { get; set; }

	[PrintDataMember]
	public decimal SummaCurr { get; set; }

	[PrintDataMember("TotalQty", DecimalPrecisionType.Quantity)]
	public decimal TotalQty { get; set; }

	[PrintDataMember("TotalCost")]
	public decimal TotalCost { get; set; }

	[PrintDataMember("TotalSum")]
	public decimal TotalSumm { get; set; }

	[PrintDataMember("MTLIST", typeof(MTIncomeRemGridRow))]
	public BindingList<MTIncomeRemGridRow> MTList { get; set; }
}
