using Ardalis.SmartEnum;
using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Common.SmartEnums;

public class FilterTypesSE : SmartEnum<FilterTypesSE, short>
{
	public static readonly FilterTypesSE IsUndefined = new FilterTypesSE("IsUndefined", -1, MultiFilterTypes.IsUndefined);

	public static readonly FilterTypesSE Tree = new FilterTypesSE("Tree", 1, MultiFilterTypes.IsTree);

	public static readonly FilterTypesSE SingleSelectTree = new FilterTypesSE("SingleSelectTree", 0, MultiFilterTypes.IsSingleSelectTree);

	public static readonly FilterTypesSE IsEqual = new FilterTypesSE("IsEqual", 2, MultiFilterTypes.Equals);

	public static readonly FilterTypesSE IsNotEqual = new FilterTypesSE("IsNotEqual", 3, MultiFilterTypes.DoesNotEqual);

	public static readonly FilterTypesSE IsGreater = new FilterTypesSE("IsGreater", 4, MultiFilterTypes.IsGreater);

	public static readonly FilterTypesSE IsGreaterOrEqual = new FilterTypesSE("IsGreaterOrEqual", 5, MultiFilterTypes.IsGreaterOrEqual);

	public static readonly FilterTypesSE IsLess = new FilterTypesSE("IsLess", 6, MultiFilterTypes.IsLess);

	public static readonly FilterTypesSE IsLessOrEqual = new FilterTypesSE("IsLessOrEqual", 7, MultiFilterTypes.IsLessOrEqual);

	public static readonly FilterTypesSE IsAnyOf = new FilterTypesSE("IsAnyOf", 8, MultiFilterTypes.IsAnyOf);

	public static readonly FilterTypesSE IsNoneOf = new FilterTypesSE("IsNoneOf", 9, MultiFilterTypes.IsNoneOf);

	public static readonly FilterTypesSE Contains = new FilterTypesSE("Contains", 10, MultiFilterTypes.Contains);

	public static readonly FilterTypesSE DoesNotContain = new FilterTypesSE("DoesNotContain", 11, MultiFilterTypes.DoesNotContain);

	public static readonly FilterTypesSE StartsWith = new FilterTypesSE("StartsWith", 12, MultiFilterTypes.StartsWith);

	public static readonly FilterTypesSE NotStartWith = new FilterTypesSE("NotStartWith", 13, MultiFilterTypes.NotStartsWith);

	public static readonly FilterTypesSE EndsWith = new FilterTypesSE("EndsWith", 14, MultiFilterTypes.EndsWith);

	public static readonly FilterTypesSE NotEndsWith = new FilterTypesSE("NotEndsWith", 15, MultiFilterTypes.NotEndsWith);

	public static readonly FilterTypesSE IsLike = new FilterTypesSE("IsLike", 16, MultiFilterTypes.IsLike);

	public static readonly FilterTypesSE IsNotLike = new FilterTypesSE("IsNotLike", 17, MultiFilterTypes.IsNotLike);

	public static readonly FilterTypesSE Blank = new FilterTypesSE("Blank", 18, MultiFilterTypes.IsBlank);

	public static readonly FilterTypesSE IsNotBlank = new FilterTypesSE("IsNotBlank", 19, MultiFilterTypes.IsNotBlank);

	public static readonly FilterTypesSE Between = new FilterTypesSE("Between", 20, MultiFilterTypes.Between);

	public static readonly FilterTypesSE NotBetween = new FilterTypesSE("NotBetween", 21, MultiFilterTypes.NotBetween);

	public MultiFilterTypes Type { get; }

	private FilterTypesSE(string name, short value, MultiFilterTypes type)
		: base(name, value)
	{
		Type = type;
	}
}
