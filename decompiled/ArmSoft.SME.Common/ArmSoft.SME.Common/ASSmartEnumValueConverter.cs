using System;
using System.Text.Json;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;

namespace ArmSoft.SME.Common;

public class ASSmartEnumValueConverter<TEnum, TValue> : SmartEnumValueConverter<TEnum, TValue> where TEnum : SmartEnum<TEnum, TValue> where TValue : IEquatable<TValue>, IComparable<TValue>, IConvertible
{
	public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
	{
		if (value == null)
		{
			writer.WriteNullValue();
		}
		else if (typeof(TValue) == typeof(byte))
		{
			writer.WriteNumberValue((byte)(object)value.Value);
		}
		else if (typeof(TValue) == typeof(sbyte))
		{
			writer.WriteNumberValue((sbyte)(object)value.Value);
		}
		else if (typeof(TValue) == typeof(ushort))
		{
			writer.WriteNumberValue((ushort)(object)value.Value);
		}
		else
		{
			base.Write(writer, value, options);
		}
	}
}
