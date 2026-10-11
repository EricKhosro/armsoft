using System.Text.Json;

namespace ArmSoft.SME.Common.Models.Caches;

public class CurrenciesCacheRow : CacheRowBaseWithName<string>
{
	public string Curr { get; set; }

	public string UnitName { get; set; }

	public string CentName { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Curr);

	public CurrenciesCacheRow(string curr, string name, string unitName, string centName, byte[] ts)
	{
		Curr = curr;
		Name = name;
		UnitName = unitName;
		CentName = centName;
		TS = ts;
	}

	public CurrenciesCacheRow()
	{
	}
}
