using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class DiscountGroupCacheRow : CacheRowBase<string>
{
	public string Code { get; set; }

	public string Caption { get; set; }

	public string Type { get; set; }

	public string TypeCaption { get; set; }

	public byte Priority { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public DiscountGroupCacheRow(string code, string caption, string type, string typeCaption, byte priority, byte[] ts)
	{
		Code = code;
		Caption = caption;
		Type = type;
		TypeCaption = typeCaption;
		Priority = priority;
		TS = ts;
	}

	public DiscountGroupCacheRow()
	{
	}
}
