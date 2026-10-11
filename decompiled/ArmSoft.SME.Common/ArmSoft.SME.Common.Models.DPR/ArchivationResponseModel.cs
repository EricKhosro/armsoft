using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.DPR;

public class ArchivationResponseModel
{
	public bool Succeeded { get; set; }

	public List<string> RowInfo { get; set; }
}
