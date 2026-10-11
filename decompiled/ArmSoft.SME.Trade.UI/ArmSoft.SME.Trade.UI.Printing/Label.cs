using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(101)]
public class Label : TradeDocumentPrintWrapper
{
	public class InnerRow : LabelRow
	{
	}

	[PrintDataMember("MTLIST", typeof(LabelRow))]
	public BindingList<LabelRow> MTList { get; set; }
}
