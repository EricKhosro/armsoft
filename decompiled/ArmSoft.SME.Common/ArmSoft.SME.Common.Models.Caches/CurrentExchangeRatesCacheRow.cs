using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.Models.Caches;

public class CurrentExchangeRatesCacheRow : CacheRowBase<string>
{
	public string Code { get; }

	public decimal Rate { get; }

	public decimal Base { get; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public CurrentExchangeRatesCacheRow()
	{
		Code = "";
		Rate = 1m;
		Base = 1m;
	}

	[JsonConstructor]
	public CurrentExchangeRatesCacheRow(string code, decimal rate, decimal @base, byte[] ts)
	{
		Code = code;
		Rate = rate;
		Base = @base;
		TS = ts;
	}
}
