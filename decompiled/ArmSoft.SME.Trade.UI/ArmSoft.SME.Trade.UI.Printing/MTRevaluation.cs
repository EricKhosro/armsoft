using System;
using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(35)]
public class MTRevaluation : TradeDocumentPrintWrapper
{
	[PrintDataMember("CompanyName")]
	public string CompanyName { get; set; }

	[PrintDataMember("ManagerFIO")]
	public string ManagerName { get; set; }

	[PrintDataMember("ChAccFIO")]
	public string ChiefAccountantFIO { get; set; }

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

	[PrintDataMember("MTLIST", typeof(MTRevaluationGridRow))]
	public BindingList<MTRevaluationGridRow> MTList { get; set; }

	[PrintDataMember]
	public BindingList<MTRevaluationExtGridRow> MTListExt { get; set; }
}
