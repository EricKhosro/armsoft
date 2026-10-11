using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Client.Data;

public interface IMultiFilterValue
{
	MultiFilterTypes FilterType { get; }

	object Value { get; }

	object EndValue { get; }
}
