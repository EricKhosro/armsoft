using System;
using ArmSoft.SME.Common.Parameters;

namespace ArmSoft.SME.Common.PersistableObject;

public class NumberFieldDefinition : FieldDefinition
{
	protected ParameterDefinition<int> _precisionParamDefinition;

	protected short _lenIntegerPart;

	public short Length { get; }

	public short Precision { get; }

	public bool IsNonNegative { get; }

	public Type Type { get; }

	public bool PrecisionDefinedInParam { get; }

	public short LengthFromParam(IParametersService parametersService)
	{
		return Convert.ToInt16(_lenIntegerPart + PrecisionFromParam(parametersService));
	}

	public short PrecisionFromParam(IParametersService parametersService)
	{
		return (short)parametersService.GetCurrentUserIntegerParam(_precisionParamDefinition);
	}

	public NumberFieldDefinition(string name, Func<string> captionDelegate, short length, short precision = 0, bool isNonNegative = true, bool isRequired = false, Type type = null, bool logChanges = true, Func<string> logCaptionDelegate = null, string publicApiProperty = "")
		: base(name, captionDelegate, isRequired, logChanges, logCaptionDelegate)
	{
		Length = length;
		Precision = precision;
		IsNonNegative = isNonNegative;
		if (type != null)
		{
			Type = type;
		}
	}

	public NumberFieldDefinition(string name, Func<string> captionDelegate, short lenIntegerPart, ParameterDefinition<int> precisionParamDefinition, bool isNonNegative = true, bool isRequired = false, Type type = null, bool logChanges = true, Func<string> logCaptionDelegate = null)
		: base(name, captionDelegate, isRequired, logChanges, logCaptionDelegate)
	{
		PrecisionDefinedInParam = true;
		_lenIntegerPart = lenIntegerPart;
		_precisionParamDefinition = precisionParamDefinition;
		IsNonNegative = isNonNegative;
		if (type != null)
		{
			Type = type;
		}
	}
}
