using System;

namespace ArmSoft.SME.Common.PersistableObject;

public class BoolFieldDefinition : FieldDefinition
{
	public BoolFieldDefinition(string name, Func<string> captionDelegate, bool isRequired = false, bool logChanges = true, Func<string> logCaptionDelegate = null, string publicApiProperty = "")
		: base(name, captionDelegate, isRequired, logChanges, logCaptionDelegate)
	{
	}
}
