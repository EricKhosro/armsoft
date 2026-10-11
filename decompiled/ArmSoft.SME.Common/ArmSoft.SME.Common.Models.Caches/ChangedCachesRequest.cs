using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Caches;

public class ChangedCachesRequest
{
	public List<ChangedCachesRequestRow> TSItems { get; set; }
}
