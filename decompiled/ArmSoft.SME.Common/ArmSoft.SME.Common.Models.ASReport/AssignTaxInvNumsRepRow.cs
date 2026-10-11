using System;

namespace ArmSoft.SME.Common.Models.ASReport;

public class AssignTaxInvNumsRepRow
{
	public DateTime Date { get; set; }

	public string TIN { get; set; }

	public decimal Sum { get; set; }

	public string Assigned { get; set; }

	public string SeralNum_NotAssignReason { get; set; }

	public string DocNumbers { get; set; }
}
