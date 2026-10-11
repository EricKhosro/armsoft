using System;
using ArmSoft.SME.Common.Parameters;

namespace ArmSoft.SME.Common.PersistableObject;

public class SumFieldByParamDefinition : NumberFieldDefinition
{
	public SumFieldByParamDefinition(string name, Func<string> captionDelegate, bool isRequired = false, bool isNonNegative = true, bool logChanges = true, Func<string> logCaptionDelegate = null, string publicApiProperty = "")
		: base(name, captionDelegate, 14, Definitions.SumPrecisionDigitsCount, isNonNegative, isRequired, null, logChanges, logCaptionDelegate)
	{
	}
}
