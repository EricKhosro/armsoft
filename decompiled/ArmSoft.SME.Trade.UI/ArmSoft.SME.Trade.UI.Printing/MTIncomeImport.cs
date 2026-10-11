using System;
using System.ComponentModel;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(19)]
public class MTIncomeImport : TradeDocumentPrintWrapper
{
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

	[PrintDataMember("Storage")]
	public StorageRow Storage { get; set; }

	[PrintDataMember("TotalQty", DecimalPrecisionType.Quantity)]
	public decimal TotalQty { get; set; }

	[PrintDataMember("TotalCost")]
	public decimal TotalCost { get; set; }

	[PrintDataMember("TotalSum")]
	public decimal TotalSumm { get; set; }

	[PrintDataMember("Mediator")]
	public string Mediator { get; set; }

	[PrintDataMember("Allower")]
	public string Allower { get; set; }

	[PrintDataMember("ChAccFIO")]
	public string ChiefAccountantFIO { get; set; }

	[PrintDataMember("Respons")]
	public string Respons { get; set; }

	[PrintDataMember("Trust")]
	public string Trust { get; set; }

	[PrintDataMember("Machine")]
	public string Machine { get; set; }

	[PrintDataMember("MTLIST", typeof(MTIncomeImportGridRow))]
	public BindingList<MTIncomeImportGridRow> MTList { get; set; }
}
