using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Common;

public class SmartEnumConverterFactory : JsonConverterFactory
{
	public override bool CanConvert(Type typeToConvert)
	{
		return typeToConvert.IsDescendantOfGenericType(typeof(SmartEnum<, >));
	}

	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		SmartEnumConverterAttribute smartEnumConverterAttribute = (SmartEnumConverterAttribute)Attribute.GetCustomAttribute(typeToConvert, typeof(SmartEnumConverterAttribute));
		Type type = null;
		Type baseType = typeToConvert.BaseType;
		while (baseType != null)
		{
			if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(SmartEnum<, >))
			{
				type = baseType.GetGenericArguments()[1];
				break;
			}
			baseType = baseType.BaseType;
		}
		Type type2 = ((smartEnumConverterAttribute != null && !smartEnumConverterAttribute.Mode.Equals(SmartEnumConverterMode.Value)) ? typeof(SmartEnumNameConverter<, >).MakeGenericType(typeToConvert, type) : typeof(ASSmartEnumValueConverter<, >).MakeGenericType(typeToConvert, type));
		return (JsonConverter)Activator.CreateInstance(type2, BindingFlags.Instance | BindingFlags.Public, null, null, null);
	}
}
