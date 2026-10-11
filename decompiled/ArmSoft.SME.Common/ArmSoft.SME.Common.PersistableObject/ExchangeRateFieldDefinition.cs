using System;

namespace ArmSoft.SME.Common.PersistableObject;

public class ExchangeRateFieldDefinition : FieldDefinition
{
	public short RateLength { get; }

	public short BaseLength { get; }

	public short RatePrecision { get; }

	public short BasePrecision { get; }

	public bool IsNonNegative { get; }

	public ExchangeRateFieldDefinition(string name, Func<string> captionDelegate, bool isRequired = false, bool isNonNegative = true, bool logChanges = true, Func<string> logCaptionDelegate = null)
		: base(name, captionDelegate, isRequired, logChanges, logCaptionDelegate)
	{
		RateLength = 11;
		BaseLength = 5;
		RatePrecision = 4;
		BasePrecision = 0;
		IsNonNegative = isNonNegative;
	}
}
