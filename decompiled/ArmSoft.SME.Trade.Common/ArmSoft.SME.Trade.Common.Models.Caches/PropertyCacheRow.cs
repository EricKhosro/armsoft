using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class PropertyCacheRow : CacheRowBase<string>
{
	public string NextProperty { get; set; }

	public string Code { get; set; }

	public string Caption { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public PropertyCacheRow(string nextProperty, string code, string caption, byte[] ts)
	{
		NextProperty = nextProperty;
		Code = code;
		Caption = caption;
		TS = ts;
	}

	public PropertyCacheRow()
	{
	}
}
