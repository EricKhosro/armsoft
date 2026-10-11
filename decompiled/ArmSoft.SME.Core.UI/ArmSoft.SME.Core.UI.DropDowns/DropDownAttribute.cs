using System;

namespace ArmSoft.SME.Core.UI.DropDowns;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DropDownAttribute : Attribute
{
	public short DropDownType { get; }

	public DropDownAttribute(short dropDownType)
	{
		DropDownType = dropDownType;
	}
}
