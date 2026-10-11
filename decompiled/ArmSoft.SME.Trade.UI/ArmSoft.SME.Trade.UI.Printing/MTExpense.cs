using System;
using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(9)]
public class MTExpense : TradeDocumentPrintWrapper
{
	public class InnerMTExpGridRow : MTExpenseGridRow
	{
	}

	public class InnerMTExpPartyGridRow : InnerMTExpGridRow
	{
	}

	[PrintDataMember("Formula")]
	public FormulaEngine Formulas { get; set; }

	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

	[PrintDataMember("OptionsTaxCode")]
	public string TaxCode { get; set; }

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("OutStorage")]
	public StorageRow StorageExpense { get; set; }

	[PrintDataMember("Mediator")]
	public string Mediator { get; set; }

	[PrintDataMember("Allower")]
	public string Allower { get; set; }

	[PrintDataMember("ChAccFIO")]
	public string ChiefAccountantFIO { get; set; }

	[PrintDataMember("Demand")]
	public string Demand { get; set; }

	[PrintDataMember("Trust")]
	public string Trust { get; set; }

	[PrintDataMember("Machine")]
	public string Machine { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("MTLIST", typeof(MTExpenseGridRow))]
	public BindingList<MTExpenseGridRow> MTList { get; set; }

	[PrintDataMember("ItemListWithParty", typeof(InnerMTExpPartyGridRow))]
	public BindingList<InnerMTExpPartyGridRow> PartyList { get; set; }
}
