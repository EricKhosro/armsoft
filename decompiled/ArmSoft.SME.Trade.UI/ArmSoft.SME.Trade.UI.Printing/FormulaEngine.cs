using System;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class FormulaEngine
{
	[PrintDataMember]
	public DateTime DateFirst { get; set; }

	[PrintDataMember]
	public DateTime DateLast { get; set; }

	[PrintDataMember]
	public decimal RoundRate { get; set; }

	[PrintDataMember]
	public int Precision { get; set; }
}
