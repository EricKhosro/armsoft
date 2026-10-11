using System;

namespace ArmSoft.SME.Core.UI.Documents;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DataViewAttribute : Attribute
{
	public short DataProviderType { get; }

	public DataViewAttribute(short dataProviderType)
	{
		DataProviderType = dataProviderType;
	}
}
