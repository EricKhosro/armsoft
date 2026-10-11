using System.Text.Json;

namespace ArmSoft.SME.Common.Models.Caches;

public class BanksCacheRow : CacheRowBaseWithName<string>
{
	public string Code { get; set; }

	public string SWIFTCODE { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public BanksCacheRow(string code, string name, string swiftCode, byte[] ts)
	{
		Code = code;
		Name = name;
		SWIFTCODE = swiftCode;
		TS = ts;
	}

	public BanksCacheRow()
	{
	}
}
