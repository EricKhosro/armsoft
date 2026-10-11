using System;
using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(14)]
public class MTReturnToSupplier : TradeDocumentPrintWrapper
{
	[PrintDataMember("Formula")]
	public FormulaEngine Formulas { get; set; }

	[PrintDataMember("Supplier")]
	public InvSupplierInfo Supplier { get; set; }

	[PrintDataMember("Buyer")]
	public InvBuyerInfo Buyer { get; set; }

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("DateInWords")]
	public string DocDateToSymbol { get; set; }

	[PrintDataMember("Curr")]
	public string Curr { get; set; }

	[PrintDataMember("Comment")]
	public string Comment { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("TotalSum")]
	public string TotalSumm { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("DocumentsTotalLabel")]
	public string DocumentsTotalLabel { get; set; }

	[PrintDataMember("SupBookNPageNRowN")]
	public string SupBookNPageNRowN { get; set; }

	[PrintDataMember("SupBookN")]
	public string SupBookN { get; set; }

	[PrintDataMember("PageN")]
	public string SupBookPageN { get; set; }

	[PrintDataMember("RowN")]
	public string SupBookRowN { get; set; }

	[PrintDataMember("ItemList", typeof(MTReturnToSupplierGridRow))]
	public BindingList<MTReturnToSupplierGridRow> ItemList { get; set; }

	[PrintDataMember("ItemListWithParty", typeof(MTReturnToSupplierGridRow))]
	public BindingList<MTReturnToSupplierGridRow> PartyList { get; set; }
}
