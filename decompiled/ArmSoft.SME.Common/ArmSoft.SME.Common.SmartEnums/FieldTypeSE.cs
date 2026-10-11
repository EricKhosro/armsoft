using System;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(15, "FieldTypes")]
public class FieldTypeSE : ASSmartEnum<FieldTypeSE, string>
{
	public const string Boolean = "Boolean";

	public const string Date = "Date";

	public const string Number = "Number";

	public const string String = "String";

	public const string Time = "Time";

	public const string TreeNode = "TreeNode";

	public static readonly FieldTypeSE BooleanType = new FieldTypeSE("Boolean", "Boolean", () => Resources.Boolean);

	public static readonly FieldTypeSE DateType = new FieldTypeSE("Date", "Date", () => Resources.DateTime);

	public static readonly FieldTypeSE NumberType = new FieldTypeSE("Number", "Number", () => Resources.NumberWord);

	public static readonly FieldTypeSE StringType = new FieldTypeSE("String", "String", () => Resources.String);

	public static readonly FieldTypeSE TimeType = new FieldTypeSE("Time", "Time", () => Resources.Time);

	public static readonly FieldTypeSE TreeNodeType = new FieldTypeSE("TreeNode", "TreeNode", () => Resources.TreeDef);

	public FieldTypeSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}

	public static SupportedType GetSupportedType(string type)
	{
		return type switch
		{
			"Boolean" => SupportedType.Boolean, 
			"Number" => SupportedType.Decimal, 
			"Date" => SupportedType.DateOnly, 
			_ => SupportedType.String, 
		};
	}
}
