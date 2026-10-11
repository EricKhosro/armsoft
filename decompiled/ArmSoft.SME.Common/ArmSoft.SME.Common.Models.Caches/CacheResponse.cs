using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Caches;

public class CacheResponse
{
	public byte[] TS { get; set; }

	public byte[] RowsMaxTS { get; set; }

	public string MergeJSON { get; set; }

	public List<string> Delete { get; set; }
}
public class CacheResponse<T> where T : CacheRowBase, new()
{
	public byte[] TS { get; set; }

	public byte[] RowsMaxTS { get; set; }

	public List<T> Merge { get; set; }

	public List<string> Delete { get; set; }
}
