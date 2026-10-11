using System;
using System.ComponentModel;
using ArmSoft.SME.Common.Printing;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(12)]
public class MTDecomplectation : TradeDocumentPrintWrapper
{
	public class InnerMTDeCmplExpGridRow : MTDeComplectationExpenseGridRow
	{
	}

	public class InnerMTDeCmplExpPartyGridRow : MTDeComplectationResultGridRow
	{
	}

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("Comment")]
	public string Comment { get; set; }

	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

	[PrintDataMember("OutStorage")]
	public StoragesCacheRow StorageExpense { get; set; }

	[PrintDataMember("InStorage")]
	public StoragesCacheRow StorageIncome { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("MTComplectation", typeof(MTDeComplectationExpenseGridRow))]
	public BindingList<MTDeComplectationExpenseGridRow> MTComplectation { get; set; }

	[PrintDataMember("Complect", typeof(MTDeComplectationResultGridRow))]
	public BindingList<MTDeComplectationResultGridRow> Complect { get; set; }

	[PrintDataMember("ComplectWithParty", typeof(InnerMTDeCmplExpPartyGridRow))]
	public BindingList<InnerMTDeCmplExpPartyGridRow> CmplPartyList { get; set; }
}
