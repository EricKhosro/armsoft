using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.DPR;

public class ImportIBTransactionsResponseModel
{
	public int CreatedCount { get; set; }

	public List<string> Errors { get; set; }
}
