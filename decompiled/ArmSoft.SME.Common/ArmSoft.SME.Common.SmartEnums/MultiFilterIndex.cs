namespace ArmSoft.SME.Common.SmartEnums;

public static class MultiFilterIndex
{
	public const short IsUndefined = -1;

	public const short IsSingleSelectTree = 0;

	public const short IsTree = 1;

	public const short IsEqual = 2;

	public const short IsNotEqual = 3;

	public const short IsGreater = 4;

	public const short IsGreaterOrEqual = 5;

	public const short IsLess = 6;

	public const short IsLessOrEqual = 7;

	public const short IsAnyOf = 8;

	public const short IsNoneOf = 9;

	public const short Contains = 10;

	public const short DoesNotContain = 11;

	public const short StartsWith = 12;

	public const short NotStartsWith = 13;

	public const short EndsWith = 14;

	public const short NotEndsWith = 15;

	public const short IsLike = 16;

	public const short IsNotLike = 17;

	public const short IsBlank = 18;

	public const short IsNotBlank = 19;

	public const short Between = 20;

	public const short NotBetween = 21;
}
