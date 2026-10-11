using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Helpers;

public static class SmartEnumHelper
{
	public static string GetPartialSmartEnumCaption(Type smartEnumType, string value)
	{
		int num = 1;
		List<string> list = new List<string>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = value;
		return GetPartialSmartEnumCodeCaption(smartEnumType, list).First().Name;
	}

	public static List<CodeName> GetSmartEnumNameCaption(Type smartEnumType, List<string> names = null)
	{
		if (DefinitionMappingsService.GetPartialSmartEnum(smartEnumType.Name) != null)
		{
			return GetPartialSmartEnumNameCaption(smartEnumType, names);
		}
		smartEnumType.TryGetValues(out var enums);
		return GetNameCaption(enums.ToList(), names);
	}

	public static List<CodeName> GetPartialSmartEnumNameCaption(Type smartEnumType, List<string> names = null)
	{
		return GetNameCaption(DefinitionMappingsService.GetPartialSmartEnum(smartEnumType.Name).GetSmartEnumFields(), names);
	}

	private static List<CodeName> GetNameCaption(List<object> allValues, List<string> names = null)
	{
		List<CodeName> list = new List<CodeName>();
		foreach (dynamic allValue in allValues)
		{
			bool flag = names == null || names.Count == 0;
			if (flag || ((flag | names.Contains(allValue.Name.ToString())) ? true : false))
			{
				list.Add(new CodeName(allValue.Name.ToString(), allValue.Caption));
			}
		}
		return list;
	}

	public static List<CodeName> GetSmartEnumCodeCaption(Type smartEnumType, List<string> values = null)
	{
		if (DefinitionMappingsService.GetPartialSmartEnum(smartEnumType.Name) != null)
		{
			return GetPartialSmartEnumCodeCaption(smartEnumType, values);
		}
		smartEnumType.TryGetValues(out var enums);
		return GetCodeCaption(enums.ToList(), values);
	}

	public static string GetSmartEnumRowCaption(Type smartEnumType, string value)
	{
		int num = 1;
		List<string> list = new List<string>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = value;
		return GetSmartEnumCodeCaption(smartEnumType, list)[0].Name;
	}

	public static List<CodeName> GetPartialSmartEnumCodeCaption(Type smartEnumType, List<string> values = null)
	{
		return GetCodeCaption(DefinitionMappingsService.GetPartialSmartEnum(smartEnumType.Name).GetSmartEnumFields(), values);
	}

	private static List<CodeName> GetCodeCaption(List<object> allValues, List<string> values = null)
	{
		List<CodeName> list = new List<CodeName>();
		foreach (dynamic allValue in allValues)
		{
			bool flag = values == null || values.Count == 0;
			if (flag || ((flag | values.Contains(allValue.Value.ToString())) ? true : false))
			{
				list.Add(new CodeName(allValue.Value.ToString(), allValue.Caption));
			}
		}
		return list;
	}

	public static string GetName<T>(Type smartEnumType, T value)
	{
		foreach (dynamic smartEnumField in DefinitionMappingsService.GetPartialSmartEnum(smartEnumType.Name).GetSmartEnumFields())
		{
			if (smartEnumField.Value == value)
			{
				return smartEnumField.Name;
			}
		}
		return null;
	}

	public static T GetValue<T>(Type smartEnumType, string name)
	{
		foreach (dynamic smartEnumField in DefinitionMappingsService.GetPartialSmartEnum(smartEnumType.Name).GetSmartEnumFields())
		{
			if (smartEnumField.Name == name)
			{
				return smartEnumField.Value;
			}
		}
		return default;
	}

	public static List<string> GetPartialSmartEnumValues(Type smartEnumType, List<string> values = null)
	{
		List<string> list = new List<string>();
		foreach (dynamic smartEnumField in DefinitionMappingsService.GetPartialSmartEnum(smartEnumType.Name).GetSmartEnumFields())
		{
			bool flag = values == null || values.Count == 0;
			if (flag || ((flag | values.Contains(smartEnumField.Value.ToString())) ? true : false))
			{
				list.Add(new string(smartEnumField.Value.ToString()));
			}
		}
		return list;
	}

	public static short GetDocumentTypeSEProviderType(short value)
	{
		foreach (dynamic smartEnumField in DefinitionMappingsService.GetPartialSmartEnum(typeof(DocumentTypeSE).Name).GetSmartEnumFields())
		{
			if (value == smartEnumField.Value)
			{
				return smartEnumField.DataProviderType;
			}
		}
		return 0;
	}

	public static short GetDataProviderUserAccessId(short dataProviderType)
	{
		foreach (dynamic smartEnumField in DefinitionMappingsService.GetPartialSmartEnum(typeof(UserAccessIdSE).Name).GetSmartEnumFields())
		{
			if (smartEnumField.RefObjectType == NavigationCommandRefObjectType.DataProvider && dataProviderType == smartEnumField.RefObjectId)
			{
				return smartEnumField.Value;
			}
		}
		return 0;
	}

	public static short? GetDropDownCacheId(string name)
	{
		foreach (dynamic smartEnumField in DefinitionMappingsService.GetPartialSmartEnum(typeof(DropDownTypeSE).Name).GetSmartEnumFields())
		{
			if (name == smartEnumField.Name)
			{
				return smartEnumField.CacheId?.Value;
			}
		}
		return null;
	}
}
