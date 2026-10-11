using System;
using System.Collections.Generic;
using System.Reflection;

namespace ArmSoft.SME.Common.Extensions;

public static class Extensions
{
	public static List<T> SplitSelected<T>(this string strList) where T : IEquatable<T>, IComparable<T>
	{
		List<T> list = new List<T>();
		string[] array = strList.Split(new char[1] { ',' });
		for (int i = 0; i < array.Length; i++)
		{
			string value = array[i].Trim();
			if (!string.IsNullOrWhiteSpace(value))
			{
				list.Add((T)Convert.ChangeType(value, typeof(T)));
			}
		}
		return list;
	}

	public static PropertyInfo ASSetValue(this object obj, string propertyName, string value)
	{
		if (propertyName.IndexOf('.') > 0)
		{
			string[] array = propertyName.Split('.');
			propertyName = array[1];
			obj = obj.GetType().GetProperty(array[0]).GetValue(obj, null);
		}
		return obj.GetType().GetProperty(propertyName)?.ASSetValue(obj, value);
	}

	public static PropertyInfo ASSetValue(this PropertyInfo propertyInfo, object obj, string value)
	{
		if (value == null)
		{
			propertyInfo.SetValue(obj, null, null);
		}
		else if (propertyInfo.PropertyType == typeof(Guid))
		{
			propertyInfo.SetValue(obj, Guid.Parse(value), null);
		}
		else if (propertyInfo.PropertyType == typeof(DateOnly) || propertyInfo.PropertyType == typeof(DateOnly?))
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				propertyInfo.SetValue(obj, null, null);
			}
			else
			{
				propertyInfo.SetValue(obj, DateOnly.Parse(value), null);
			}
		}
		else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				propertyInfo.SetValue(obj, null, null);
			}
			else
			{
				propertyInfo.SetValue(obj, DateTime.Parse(value), null);
			}
		}
		else if (propertyInfo.PropertyType == typeof(bool))
		{
			propertyInfo.SetValue(obj, bool.Parse(value), null);
		}
		else
		{
			propertyInfo.SetValue(obj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
		}
		return propertyInfo;
	}

	public static object ASChangeType(this string value, Type type)
	{
		if (value == null)
		{
			return null;
		}
		if (type == typeof(Guid))
		{
			return Guid.Parse(value);
		}
		if (type == typeof(DateOnly) || type == typeof(DateOnly?))
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			return DateOnly.Parse(value);
		}
		return Convert.ChangeType(value, type);
	}
}
