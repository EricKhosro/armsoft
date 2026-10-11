using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class InnerMTMoveGridRow : MTMoveGridRow
{
	[PrintDataMember]
	public int MTID { get; set; }

	[PrintDataMember]
	public int RowID { get; set; }

	[PrintDataMember("Unit")]
	public string Unit { get; set; }
}
