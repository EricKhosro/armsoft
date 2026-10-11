using System;
using System.Text.Json;

namespace ArmSoft.SME.Common.Models.Caches;

public class CPACodesCacheRow : CacheRowBaseWithName<string>
{
	public string Code { get; set; } = string.Empty;

	public string Type { get; set; } = string.Empty;

	public string TypeName { get; set; } = string.Empty;

	public DateOnly? MarkInitDate { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public bool Matched(object key)
	{
		return key.ToString() == Code;
	}

	public CPACodesCacheRow()
	{
	}

	public CPACodesCacheRow(string code, string name, string type, string typeName, DateOnly? markInitDate, byte[] ts)
	{
		Code = code;
		Name = name;
		Type = type;
		TypeName = typeName;
		MarkInitDate = markInitDate;
		TS = ts;
	}

	public CPACodesCacheRow Clone()
	{
		return new CPACodesCacheRow(Code, Name, Type, TypeName, MarkInitDate, TS);
	}
}
