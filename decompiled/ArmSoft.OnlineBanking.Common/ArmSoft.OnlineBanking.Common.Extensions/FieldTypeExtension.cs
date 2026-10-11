using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArmSoft.OnlineBanking.Common.Attributes;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Extensions;

public static class FieldTypeExtension
{
	private static readonly Dictionary<FieldType, Type> FieldValueTypes = new Dictionary<FieldType, Type>();

	public static Type GetFieldValueType(this FieldType fieldType)
	{
		if (!FieldValueTypes.ContainsKey(fieldType))
		{
			MemberInfo memberInfo = typeof(FieldType).GetMembers().FirstOrDefault((MemberInfo x) => x.Name == fieldType.ToString());
			if (memberInfo == null)
			{
				FieldValueTypes[fieldType] = typeof(object);
			}
			else
			{
				CustomizationFieldAttribute customAttribute = memberInfo.GetCustomAttribute<CustomizationFieldAttribute>();
				if (customAttribute == null)
				{
					FieldValueTypes[fieldType] = typeof(object);
				}
				else
				{
					FieldValueTypes[fieldType] = customAttribute.Type;
				}
			}
		}
		return FieldValueTypes[fieldType];
	}
}
