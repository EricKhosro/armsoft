using System;

namespace ArmSoft.SME.Common.PersistableObject;

public class StringFieldDefinition : FieldDefinition
{
	public short Length { get; }

	public StringFieldDefinition(string name, Func<string> captionDelegate, short length, bool isRequired = false, bool logChanges = true, Func<string> logCaptionDelegate = null, string publicApiProperty = "")
		: base(name, captionDelegate, isRequired, logChanges, logCaptionDelegate)
	{
		Length = length;
	}
}
