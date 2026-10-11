namespace ArmSoft.SME.Core.UI;

public enum SelectCompareOperations
{
	NoOperation,
	Equal,
	NotEqual,
	Less,
	LessE,
	Greater,
	GreaterE,
	LikeMask,
	NotLikeMask,
	ValueIsNullOrEmpty,
	ValueIsNotNullAndNotEmpty,
	Between,
	NotBetween,
	InOperator,
	NotIn,
	StartsWith,
	EndsWith
}
