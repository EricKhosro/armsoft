using System;
using System.Text.Json.Serialization;
using Ardalis.SmartEnum;

namespace ArmSoft.SME.Common.SmartEnums;

public abstract class ASSmartEnum<TEnum, TValue> : SmartEnum<TEnum, TValue> where TEnum : ASSmartEnum<TEnum, TValue> where TValue : IEquatable<TValue>, IComparable<TValue>
{
	[JsonIgnore]
	public readonly Func<string> CaptionDelegate;

	public string Caption => CaptionDelegate();

	protected ASSmartEnum(string name, TValue value, Func<string> captionDelegate)
		: base(name, value)
	{
		CaptionDelegate = captionDelegate;
	}

	protected ASSmartEnum(string name, TValue value)
		: base(name, value)
	{
		CaptionDelegate = () => string.Empty;
	}
}
