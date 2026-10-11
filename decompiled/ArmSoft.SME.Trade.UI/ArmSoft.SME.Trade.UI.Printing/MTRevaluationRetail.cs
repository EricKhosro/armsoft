using System;
using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(23)]
public class MTRevaluationRetail : TradeDocumentPrintWrapper
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

	[PrintDataMember("Storage")]
	public StorageRow Storage { get; set; }

	[PrintDataMember("MTLIST", typeof(MTRevaluationRetailGridRow))]
	public BindingList<MTRevaluationRetailGridRow> MTList { get; set; }
}
