using System;
using ArmSoft.PrintTemplates;
using Armsoft.PrintTemplates2.Resources;

namespace Armsoft.PrintTemplates2.Helper;

internal static class ConverterHelper
{
	public static T ToGeneric<T>(this object value)
	{
		if (value == null)
		{
			return default;
		}
		Type typeFromHandle = typeof(T);
		Type type = Nullable.GetUnderlyingType(typeFromHandle) ?? typeFromHandle;
		if (type == typeof(TimeSpan))
		{
			if (TimeSpan.TryParse(value.ToString(), out var result))
			{
				return (T)(object)result;
			}
			throw new ArgumentException(Resource.UnableToCastTimeSpan);
		}
		try
		{
			return (T)Convert.ChangeType(value, type);
		}
		catch (Exception ex)
		{
			throw new InvalidTemplateException(string.Format(Resource.InvalidConvertValue, value.GetType(), type, ex.Message));
		}
	}
}
