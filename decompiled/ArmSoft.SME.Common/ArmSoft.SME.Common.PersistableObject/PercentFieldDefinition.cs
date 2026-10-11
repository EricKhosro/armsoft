using System;

namespace ArmSoft.SME.Common.PersistableObject;

public class PercentFieldDefinition : NumberFieldDefinition
{
	public PercentFieldDefinition(string name, Func<string> captionDelegate, bool isRequired = false, bool isNonNegative = true, bool logChanges = true, Func<string> logCaptionDelegate = null, string publicApiProperty = "")
		: base(name, captionDelegate, 6, 2, isNonNegative, isRequired, null, logChanges, logCaptionDelegate)
	{
	}
}
