using System;
using System.Collections.Generic;
using System.Reflection;
using ArmSoft.SME.Common.Directories;

namespace ArmSoft.SME.Common.Helpers;

public class PODefinitionHelper : IPODefinitionHelper
{
	public virtual List<short> GetExchangableDirectoryTypes()
	{
		return GetExchangablePOTypes<AllDirectoryDefinitions>();
	}

	public virtual List<short> GetLoggablePOTypes()
	{
		return GetLoggablePOTypes<AllDirectoryDefinitions>();
	}

	protected List<short> GetExchangablePOTypes<T>()
	{
		List<short> list = new List<short>();
		PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Static | BindingFlags.Public);
		foreach (PropertyInfo propertyInfo in properties)
		{
			object value = propertyInfo.GetValue(null);
			if (Convert.ToBoolean(propertyInfo.PropertyType.GetProperty("IsExchangeable").GetValue(value)))
			{
				short item = (short)propertyInfo.PropertyType.GetProperty("POType").GetValue(value);
				list.Add(item);
			}
		}
		return list;
	}

	protected List<short> GetLoggablePOTypes<T>()
	{
		List<short> list = new List<short>();
		PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Static | BindingFlags.Public);
		foreach (PropertyInfo propertyInfo in properties)
		{
			object value = propertyInfo.GetValue(null);
			if (Convert.ToBoolean(propertyInfo.PropertyType.GetProperty("AllowLog").GetValue(value)))
			{
				short item = (short)propertyInfo.PropertyType.GetProperty("POType").GetValue(value);
				list.Add(item);
			}
		}
		return list;
	}
}
