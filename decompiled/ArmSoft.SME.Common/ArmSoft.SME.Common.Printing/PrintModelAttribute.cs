using System;

namespace ArmSoft.SME.Common.Printing;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PrintModelAttribute : Attribute
{
	public short Type { get; }

	public PrintModelAttribute(short type)
	{
		Type = type;
	}
}
