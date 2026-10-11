using System;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(15, "FieldTypes")]
public class FieldTypeExtSimpleModeSE : ASSmartEnum<FieldTypeExtSimpleModeSE, string>
{
	public const string BooleanConst = "Boolean";

	public const string DateConst = "Date";

	public const string NumberConst = "Number";

	public const string StringConst = "String";

	public const string TimeConst = "Time";

	public const string DropDownConst = "DropDown";

	public static readonly FieldTypeExtSimpleModeSE BooleanType = new FieldTypeExtSimpleModeSE("Boolean", "Boolean", () => Resources.Boolean);

	public static readonly FieldTypeExtSimpleModeSE DateType = new FieldTypeExtSimpleModeSE("Date", "Date", () => Resources.DateTime);

	public static readonly FieldTypeExtSimpleModeSE NumberType = new FieldTypeExtSimpleModeSE("Number", "Number", () => Resources.NumberWord);

	public static readonly FieldTypeExtSimpleModeSE StringType = new FieldTypeExtSimpleModeSE("String", "String", () => Resources.String);

	public static readonly FieldTypeExtSimpleModeSE TimeType = new FieldTypeExtSimpleModeSE("Time", "Time", () => Resources.Time);

	public static readonly FieldTypeExtSimpleModeSE DropDownType = new FieldTypeExtSimpleModeSE("DropDown", "DropDown", () => Resources.DropDown);

	public FieldTypeExtSimpleModeSE(string name, string value, Func<string> captionDelegate)
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
