using System;
using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(21)]
public class MTInvoiceRetail : TradeDocumentPrintWrapper
{
	[PrintDataMember("Formula")]
	public FormulaEngine Formulas { get; set; }

	[PrintDataMember("Supplier")]
	public InvSupplierInfo Supplier { get; set; }

	[PrintDataMember("Buyer")]
	public InvBuyerInfo Buyer { get; set; }

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DateInWords")]
	public string DocDateToSymbol { get; set; }

	[PrintDataMember("Curr")]
	public string Curr { get; set; }

	[PrintDataMember("Comment")]
	public string Comment { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("Storage")]
	public StorageRow Storage { get; set; }

	[PrintDataMember("TotalSum")]
	public string TotalSumm { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("DocumentsTotalLabel")]
	public string DocumentsTotalLabel { get; set; }

	[PrintDataMember("Payer")]
	public string Payer { get; set; }

	[PrintDataMember("UserDescription")]
	public string UserDescription { get; set; }

	[PrintDataMember("ItemList", typeof(MTInvoiceRetailGridRow))]
	public BindingList<MTInvoiceRetailGridRow> ItemList { get; set; }

	[PrintDataMember("ItemListWithParty", typeof(MTInvoiceRetailGridRow))]
	public BindingList<MTInvoiceRetailGridRow> PartyList { get; set; }

	[PrintDataMember]
	public string SummInWords { get; set; }

	[PrintDataMember]
	public string SummInWordsAMD { get; set; }
}
