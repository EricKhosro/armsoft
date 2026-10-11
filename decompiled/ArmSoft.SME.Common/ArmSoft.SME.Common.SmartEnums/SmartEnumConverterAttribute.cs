using System;

namespace ArmSoft.SME.Common.SmartEnums;

[AttributeUsage(AttributeTargets.Class)]
public class SmartEnumConverterAttribute : Attribute
{
	public SmartEnumConverterMode Mode { get; set; }

	public SmartEnumConverterAttribute()
	{
	}

	public SmartEnumConverterAttribute(SmartEnumConverterMode mode)
	{
		Mode = mode;
	}
}
