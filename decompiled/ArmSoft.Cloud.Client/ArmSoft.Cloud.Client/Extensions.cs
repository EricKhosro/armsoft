using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ArmSoft.Cloud.Client;

public static class Extensions
{
	public static string ToQueryString(this object source, string separator = ";")
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		List<KeyValuePair<string, object>> source2 = source.ToListOfProperties();
		return string.Join("&", source2.Select((KeyValuePair<string, object> x) => Uri.EscapeDataString(x.Key) + "=" + Uri.EscapeDataString(x.Value.ToString())));
	}

	private static List<KeyValuePair<string, object>> ToListOfProperties(this object source, string parentObjectName = "")
	{
		Dictionary<string, object> dictionary = (from x in source.GetType().GetProperties()
			where x.CanRead
			where x.GetValue(source, null) != null
			select x).ToDictionary((PropertyInfo x) => x.Name, (PropertyInfo x) => x.GetValue(source, null));
		List<string> list = dictionary.Select((KeyValuePair<string, object> x) => x.Key).ToList();
		List<KeyValuePair<string, object>> list2 = new List<KeyValuePair<string, object>>();
		foreach (string item in list)
		{
			string key = (string.IsNullOrEmpty(parentObjectName) ? item : (parentObjectName + "." + item));
			Type type = dictionary[item].GetType();
			Type type2 = (type.IsGenericType ? type.GetGenericArguments()[0] : type.GetElementType());
			if (type2 != null && (type2.IsPrimitive || type2.IsEnum || type2 == typeof(string)))
			{
				foreach (object item2 in dictionary[item] as IEnumerable)
				{
					list2.Add(new KeyValuePair<string, object>(key, item2));
				}
			}
			else if (type == typeof(DateTime))
			{
				list2.Add(new KeyValuePair<string, object>(key, ((DateTime)dictionary[item]).ToString("s", CultureInfo.InvariantCulture)));
			}
			else if (!type.IsPrimitive && !type.IsEnum && type != typeof(string) && type != typeof(DateTime))
			{
				list2.AddRange(dictionary[item].ToListOfProperties(item));
			}
			else
			{
				list2.Add(new KeyValuePair<string, object>(key, dictionary[item]));
			}
		}
		return list2;
	}

	public static string GetProps<T>(Expression<Func<T, object>> parameters)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (parameters != null && parameters.Body != null)
		{
			NewExpression newExpression = parameters.Body as NewExpression;
			if (newExpression.Members != null && newExpression.Members.Count != 0)
			{
				foreach (MemberInfo member in newExpression.Members)
				{
					stringBuilder.Append(member.Name + ",");
				}
			}
		}
		return stringBuilder.ToString();
	}

	public static void AddPathParameterValue(this StringBuilder uriBuilder, string pathParameterName, string pathParameterValue)
	{
		uriBuilder.Replace(pathParameterName, Uri.EscapeDataString(pathParameterValue));
	}

	public static void AddPathParameterValue(this StringBuilder uriBuilder, string pathParameterName, int pathParameterValue)
	{
		uriBuilder.Replace(pathParameterName, Uri.EscapeDataString(Convert.ToString(pathParameterValue, CultureInfo.InvariantCulture)));
	}

	public static void AddPathParameterValue(this StringBuilder uriBuilder, string pathParameterName, DateTime pathParameterValue)
	{
		uriBuilder.Replace(pathParameterName, Uri.EscapeDataString(pathParameterValue.ToString("s", CultureInfo.InvariantCulture)));
	}

	public static void AddQueryParameter(this StringBuilder uriBuilder, object queryParameter)
	{
		uriBuilder.Append("?" + queryParameter.ToQueryString());
	}
}
