using System;
using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(22)]
public class MTStockTakingRetail : TradeDocumentPrintWrapper
{
	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

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

	[PrintDataMember("Storage")]
	public StorageRow Storage { get; set; }

	[PrintDataMember("TotalActualQuantity")]
	public decimal TotalActualQuantity { get; set; }

	[PrintDataMember("TotalActualQuantityInWords")]
	public string TotalActualQuantityInWords { get; set; }

	[PrintDataMember("TotalActualQuantityInWordsEng")]
	public string TotalActualQuantityInWordsEng { get; set; }

	[PrintDataMember("TotalActualValue")]
	public decimal TotalActualValue { get; set; }

	[PrintDataMember("TotalActualValueInWords")]
	public string TotalActualValueInWords { get; set; }

	[PrintDataMember("TotalActualValueInWordsEng")]
	public string TotalActualValueInWordsEng { get; set; }

	[PrintDataMember("TotalAccQuantity")]
	public decimal TotalAccQuantity { get; set; }

	[PrintDataMember("TotalAccValue")]
	public decimal TotalAccValue { get; set; }

	[PrintDataMember("TotalDeficitQuantity")]
	public decimal TotalDeficitQuantity { get; set; }

	[PrintDataMember("TotalDeficitSumm")]
	public decimal TotalDeficitSumm { get; set; }

	[PrintDataMember("TotalSurplusQuantity")]
	public decimal TotalSurplusQuantity { get; set; }

	[PrintDataMember("TotalSurplusSumm")]
	public decimal TotalSurPlusSumm { get; set; }

	[PrintDataMember("MTLIST", typeof(MTStockTakingMTListGridRow))]
	public BindingList<MTStockTakingMTListGridRow> MTList { get; set; }

	[PrintDataMember("MTColStList")]
	public BindingList<MTColStListGridRow> MTColStList { get; set; }

	[PrintDataMember("RowCountInWords")]
	public string RowCountInWords { get; set; }

	[PrintDataMember("RowCountInWordsEng")]
	public string RowCountInWordsEng { get; set; }

	[PrintDataMember("RowCount")]
	public string RowCount { get; set; }
}
