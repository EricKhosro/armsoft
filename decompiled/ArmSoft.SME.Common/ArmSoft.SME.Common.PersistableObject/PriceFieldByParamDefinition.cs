using System;
using ArmSoft.SME.Common.Parameters;

namespace ArmSoft.SME.Common.PersistableObject;

public class PriceFieldByParamDefinition : NumberFieldDefinition
{
	public PriceFieldByParamDefinition(string name, Func<string> captionDelegate, bool isRequired = false, bool logChanges = true, Func<string> logCaptionDelegate = null)
		: base(name, captionDelegate, 14, Definitions.SumPrecisionDigitsCount, isNonNegative: true, isRequired, null, logChanges, logCaptionDelegate)
	{
	}
}
