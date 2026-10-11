using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Caches;

public class CacheRequest
{
	public required short CacheId { get; set; }

	public byte[] TS { get; set; }

	public byte[] RowsMaxTS { get; set; }

	public List<CacheRequestRow> TSItems { get; set; }
}
