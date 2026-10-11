using System;
using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(32)]
public class MTPurchaseOrder : TradeDocumentPrintWrapper
{
	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("Supplier")]
	public PartnerInfo Supplier { get; set; }

	[PrintDataMember("Buyer")]
	public InvBuyerInfo Buyer { get; set; }

	[PrintDataMember("Email")]
	public string Email { get; set; }

	[PrintDataMember("Comment")]
	public string Comment { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("MTLIST", typeof(MTPurchaseOrderGridRow))]
	public BindingList<MTPurchaseOrderGridRow> MTList { get; set; }
}
