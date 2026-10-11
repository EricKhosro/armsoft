using System;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.Parameters;

public class ParameterDefinition<T> : ParameterDefinition
{
	public ParameterDefinition(short id, Func<string> captionDelegate, bool userParam, bool roleParam, bool useFromCacheService, bool useFromCacheUI, T defaultValue)
		: base(id, captionDelegate, userParam, roleParam, useFromCacheService, useFromCacheUI, GetDefaultValueString(defaultValue), typeof(T))
	{
	}

	public ParameterDefinition(short id, Func<string> captionDelegate, bool userParam, bool roleParam, bool useFromCacheService, bool useFromCacheUI)
		: base(id, captionDelegate, userParam, roleParam, useFromCacheService, useFromCacheUI, "", typeof(T))
	{
	}

	private static string GetDefaultValueString(T defaultValue)
	{
		if (defaultValue == null)
		{
			return " ";
		}
		if (defaultValue is DateOnly value)
		{
			return Convertor.ConvertDateOnlyToString(value);
		}
		if (defaultValue is DateTime value2)
		{
			return Convertor.ConvertDateToString(value2);
		}
		return defaultValue.ToString();
	}
}
public class ParameterDefinition
{
	public readonly short Id;

	public readonly bool UseFromCacheService;

	public readonly bool UseFromCacheUI;

	public readonly bool UserParam;

	public readonly bool RoleParam;

	public readonly string DefaultValue;

	public readonly Type Type;

	[JsonIgnore]
	private readonly Func<string> _captionDelegate;

	public string Caption => _captionDelegate();

	public ParameterDefinition(short id, Func<string> captionDelegate, bool userParam, bool roleParam, bool useFromCacheService, bool useFromCacheUI, string defaultValue, Type type)
	{
		Id = id;
		_captionDelegate = captionDelegate;
		UserParam = userParam;
		RoleParam = roleParam;
		UseFromCacheService = useFromCacheService;
		UseFromCacheUI = useFromCacheUI;
		DefaultValue = defaultValue;
		Type = type;
	}
}
