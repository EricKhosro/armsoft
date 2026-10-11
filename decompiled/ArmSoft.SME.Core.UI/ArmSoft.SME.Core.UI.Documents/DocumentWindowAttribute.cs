using System;

namespace ArmSoft.SME.Core.UI.Documents;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DocumentWindowAttribute : Attribute
{
	public short DocType { get; }

	public DocumentWindowAttribute(short docType)
	{
		DocType = docType;
	}
}
