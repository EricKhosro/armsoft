using System;

namespace ArmSoft.SME.Core.UI.Directories;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DirectoryWindowAttribute : Attribute
{
	public short DirectoryType { get; }

	public DirectoryWindowAttribute(short directoryType)
	{
		DirectoryType = directoryType;
	}
}
