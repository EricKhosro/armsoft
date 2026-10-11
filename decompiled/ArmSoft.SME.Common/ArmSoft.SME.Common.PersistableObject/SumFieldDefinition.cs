using System;

namespace ArmSoft.SME.Common.PersistableObject;

public class SumFieldDefinition : NumberFieldDefinition
{
	public SumFieldDefinition(string name, Func<string> captionDelegate, bool isRequired = false, bool isNonNegative = true, bool logChanges = true, Func<string> logCaptionDelegate = null, string publicApiProperty = "")
		: base(name, captionDelegate, 16, 2, isNonNegative, isRequired, null, logChanges, logCaptionDelegate)
	{
	}
}
