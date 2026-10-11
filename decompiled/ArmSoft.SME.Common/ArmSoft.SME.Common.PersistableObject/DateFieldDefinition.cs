using System;

namespace ArmSoft.SME.Common.PersistableObject;

public class DateFieldDefinition : FieldDefinition
{
	public DateFieldDefinition(string name, Func<string> captionDelegate, bool isRequired = false, bool logChanges = true, Func<string> logCaptionDelegate = null, string publicApiProperty = "")
		: base(name, captionDelegate, isRequired, logChanges, logCaptionDelegate)
	{
	}
}
