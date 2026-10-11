using System.ComponentModel;
using System.Reflection;
using ArmSoft.SME.Common.Printing;
using DevExpress.Data;

namespace ArmSoft.SME.Core.UI.Printing;

public class DesignerBinding<T> : BindingList<T>, IDisplayNameProvider
{
	private readonly string _displayName;

	public string GetDataSourceDisplayName()
	{
		return _displayName;
	}

	public string GetFieldDisplayName(string[] fieldAccessors)
	{
		string result = fieldAccessors[fieldAccessors.GetUpperBound(0)];
		PrintDataMemberAttribute printDataMemberAttribute = GetPropertyInfo(fieldAccessors)?.GetCustomAttribute<PrintDataMemberAttribute>();
		if (printDataMemberAttribute != null)
		{
			string captionResourceId = printDataMemberAttribute.CaptionResourceId;
			if (!string.IsNullOrWhiteSpace(captionResourceId))
			{
				return Settings.StringResourceService.GetStringResource(captionResourceId);
			}
			return result;
		}
		return result;
	}

	private static PropertyInfo GetPropertyInfo(string[] fieldAccessors)
	{
		int num = fieldAccessors.GetLowerBound(0);
		PropertyInfo propertyInfo = typeof(T).GetProperty(fieldAccessors[num]);
		while (num != fieldAccessors.GetUpperBound(0))
		{
			num++;
			if (propertyInfo == null)
			{
				return null;
			}
			propertyInfo = ((!(((PrintDataMemberAttribute[])propertyInfo.GetCustomAttributes(typeof(PrintDataMemberAttribute), inherit: false))[0].ObjectType == null)) ? ((PrintDataMemberAttribute[])propertyInfo.GetCustomAttributes(typeof(PrintDataMemberAttribute), inherit: false))[0].ObjectType.GetProperty(fieldAccessors[num]) : propertyInfo.PropertyType.GetProperty(fieldAccessors[num]));
		}
		return propertyInfo;
	}

	public DesignerBinding(string displayName)
	{
		_displayName = displayName;
	}
}
