using System.Text.Json;

namespace ArmSoft.SME.Common.Models.Caches;

public class CodeCaptionPairsCacheRow : CacheRowBaseWithName<string>
{
	public string Code { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public CodeCaptionPairsCacheRow()
	{
		Code = string.Empty;
		Name = string.Empty;
	}

	public CodeCaptionPairsCacheRow(string code, string name, byte[] ts)
	{
		Code = code;
		Name = name;
		TS = ts;
	}

	public CodeCaptionPairsCacheRow Clone()
	{
		return new CodeCaptionPairsCacheRow(Code, Name, TS);
	}
}
