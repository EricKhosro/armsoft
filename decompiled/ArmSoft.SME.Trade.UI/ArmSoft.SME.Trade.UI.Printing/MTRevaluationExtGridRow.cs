using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTRevaluationExtGridRow : MTRevaluationGridRow
{
	[PrintDataMember]
	public decimal OldSum { get; set; }

	[PrintDataMember]
	public decimal DiffSum { get; set; }

	[PrintDataMember]
	public decimal DiffPrice { get; set; }

	[PrintDataMember]
	public decimal NewSum { get; set; }

	[PrintDataMember]
	public decimal OldSumVAT { get; set; }

	[PrintDataMember]
	public decimal DiffSumVAT { get; set; }

	[PrintDataMember]
	public decimal DiffPriceVAT { get; set; }

	[PrintDataMember]
	public decimal NewSumVAT { get; set; }

	[PrintDataMember]
	public decimal OldCurSum { get; set; }

	[PrintDataMember]
	public decimal DiffCurSum { get; set; }

	[PrintDataMember]
	public decimal DiffCurPrice { get; set; }

	[PrintDataMember]
	public decimal NewCurSum { get; set; }
}
