using System;

namespace ArmSoft.SME.Common.Enums;

[Flags]
public enum MultiFilterTypes
{
	IsUndefined = 0,
	IsTree = 1,
	Equals = 2,
	DoesNotEqual = 4,
	IsGreater = 8,
	IsGreaterOrEqual = 0x10,
	IsLess = 0x20,
	IsLessOrEqual = 0x40,
	IsAnyOf = 0x80,
	IsNoneOf = 0x100,
	Contains = 0x200,
	DoesNotContain = 0x400,
	StartsWith = 0x800,
	EndsWith = 0x1000,
	IsLike = 0x2000,
	IsNotLike = 0x4000,
	IsBlank = 0x8000,
	IsNotBlank = 0x10000,
	NotStartsWith = 0x20000,
	NotEndsWith = 0x40000,
	IsSingleSelectTree = 0x80000,
	Between = 0x100000,
	NotBetween = 0x200000,
	All = IsTree | Equals | DoesNotEqual | IsGreater | IsGreaterOrEqual | IsLess | IsLessOrEqual | IsAnyOf | IsNoneOf | Contains | DoesNotContain | StartsWith | EndsWith | IsLike | IsNotLike | IsBlank | IsNotBlank | NotStartsWith | NotEndsWith | Between | NotBetween,
	Common = 0x37FFFE,
	AllWithSingleSelectTree = Common | IsSingleSelectTree,
	CommonExceptMultiSelect = 0x37FE7E,
	EqualsAndIsAnyOf = 0x82,
	AllExceptBlanks = 0x367FFF,
	CommonExceptMultiSelectAndBlanks = 0x367E7E,
	CommonExceptBlanks = 0x367FFE,
	Date = 0x31807E,
	DateExceptBlanks = 0x30007E,
	IsAnyOfAndIsLike = 0x2080
}
