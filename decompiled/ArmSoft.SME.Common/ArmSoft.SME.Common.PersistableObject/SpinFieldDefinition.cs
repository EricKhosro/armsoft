using System;

namespace ArmSoft.SME.Common.PersistableObject;

public class SpinFieldDefinition : NumberFieldDefinition
{
	public decimal MinValue { get; }

	public decimal MaxValue { get; }

	public SpinFieldDefinition(string name, Func<string> captionDelegate, bool isRequired = false, decimal minValue = 0m, decimal maxValue = 2m, bool isNonNegative = false, Type type = null, bool logChanges = true, Func<string> logCaptionDelegate = null)
		: base(name, captionDelegate, 16, 2, isNonNegative, isRequired, type, logChanges, logCaptionDelegate)
	{
		MinValue = minValue;
		MaxValue = maxValue;
	}
}
