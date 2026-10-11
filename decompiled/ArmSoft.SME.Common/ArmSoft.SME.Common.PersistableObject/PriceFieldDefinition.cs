using System;

namespace ArmSoft.SME.Common.PersistableObject;

public class PriceFieldDefinition : NumberFieldDefinition
{
	public PriceFieldDefinition(string name, Func<string> captionDelegate, bool isRequired = false, bool logChanges = true, Func<string> logCaptionDelegate = null, string publicApiProperty = "")
		: base(name, captionDelegate, 16, 2, isNonNegative: true, isRequired, null, logChanges, logCaptionDelegate)
	{
	}
}
