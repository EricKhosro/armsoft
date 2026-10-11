using System;

namespace ArmSoft.SME.Common.PersistableObject;

public class QuantityFieldDefinition : NumberFieldDefinition
{
	public QuantityFieldDefinition(string name, Func<string> captionDelegate, bool isRequired = false, bool isNonNegative = true, bool logChanges = true, Func<string> logCaptionDelegate = null, string publicApiProperty = "")
		: base(name, captionDelegate, 13, 3, isNonNegative, isRequired, null, logChanges, logCaptionDelegate)
	{
	}
}
