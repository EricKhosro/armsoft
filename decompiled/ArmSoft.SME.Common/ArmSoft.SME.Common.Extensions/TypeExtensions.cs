using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArmSoft.SME.Common.Extensions;

public static class TypeExtensions
{
	public static bool IsDescendantOfGenericType(this Type type, Type genericType)
	{
		while (type != null)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
			{
				return true;
			}
			type = type.BaseType;
		}
		return false;
	}

	public static List<object> GetSmartEnumFields(this Type type)
	{
		return (from pi in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
			where type.IsAssignableFrom(pi.FieldType)
			select pi.GetValue(null)).ToList();
	}

	public static bool IsNumeric(this Type numberType)
	{
		if (!(numberType == typeof(byte)) && !(numberType == typeof(sbyte)) && !(numberType == typeof(ushort)) && !(numberType == typeof(uint)) && !(numberType == typeof(ulong)) && !(numberType == typeof(short)) && !(numberType == typeof(int)) && !(numberType == typeof(long)) && !(numberType == typeof(decimal)) && !(numberType == typeof(double)) && !(numberType == typeof(float)) && !(numberType == typeof(short?)) && !(numberType == typeof(int?)) && !(numberType == typeof(long?)))
		{
			return numberType == typeof(decimal?);
		}
		return true;
	}
}
