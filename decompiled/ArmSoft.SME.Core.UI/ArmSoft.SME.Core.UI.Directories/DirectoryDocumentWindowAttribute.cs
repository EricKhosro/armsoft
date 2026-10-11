using System;

namespace ArmSoft.SME.Core.UI.Directories;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DirectoryDocumentWindowAttribute : Attribute
{
	public short DirectoryType { get; }

	public DirectoryDocumentWindowAttribute(short directoryType)
	{
		DirectoryType = directoryType;
	}
}
