using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.DPR;

public class UpdateCPACodesFromB2BResponse
{
	public int CreatedCount { get; set; }

	public int EditedCount { get; set; }

	public List<string> Exceptions { get; set; }
}
