using System;

namespace ArmSoft.SME.Core.UI.VersionManagement;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ChangeAttribute : Attribute
{
	public short Number { get; }

	public ChangeAttribute(short number)
	{
		Number = number;
	}
}
