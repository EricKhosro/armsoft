using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Client.Data;

public struct MultiFilterValue<T>(T value, MultiFilterTypes filterType, T endValue = default(T)) : IMultiFilterValue
{
	public T Value { get; set; } = value;

	public T EndValue { get; set; } = endValue;

	public MultiFilterTypes FilterType { get; set; } = filterType;

	object IMultiFilterValue.Value => Value;

	object IMultiFilterValue.EndValue => EndValue;
}
