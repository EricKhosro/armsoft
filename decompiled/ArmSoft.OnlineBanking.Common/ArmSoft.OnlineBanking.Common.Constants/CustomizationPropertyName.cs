using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArmSoft.OnlineBanking.Common.Attributes;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Constants;

public static class CustomizationPropertyName
{
	public const string DefaultValue = "DefaultValue";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string DisplayedValues = "DisplayedValues";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string AllowedValues = "AllowedValues";

	[CustomizationFieldOption(Type = typeof(bool?))]
	public const string IsHidden = "IsHidden";

	[CustomizationFieldOption(Type = typeof(bool?))]
	public const string IsRequired = "IsRequired";

	[CustomizationFieldOption(Type = typeof(bool?))]
	public const string IsEnabled = "IsEnabled";

	[CustomizationFieldOption(Type = typeof(int?))]
	public const string MinLength = "MinLength";

	[CustomizationFieldOption(Type = typeof(int?))]
	public const string MaxLength = "MaxLength";

	[CustomizationFieldOption(Type = typeof(decimal?))]
	public const string MinValue = "MinValue";

	[CustomizationFieldOption(Type = typeof(decimal?))]
	public const string MaxValue = "MaxValue";

	[CustomizationFieldOption(Type = typeof(bool?))]
	public const string MinValueExclusive = "MinValueExclusive";

	[CustomizationFieldOption(Type = typeof(bool?))]
	public const string MaxValueExclusive = "MaxValueExclusive";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string FilterCurrency = "FilterCurrency";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string FilterCardTypes = "FilterCardTypes";

	[CustomizationFieldOption(Type = typeof(List<AccountType>))]
	public const string FilterAccountTypes = "FilterAccountTypes";

	[CustomizationFieldOption(Type = typeof(List<CreditType>))]
	public const string FilterCreditTypes = "FilterCreditTypes";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string DirectoryType = "DirectoryType";

	[CustomizationFieldOption(Type = typeof(bool?))]
	public const string ShowEmptyValue = "ShowEmptyValue";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string EmptyValueLabel = "EmptyValueLabel";

	[CustomizationFieldOption(Type = typeof(int?))]
	public const string Precision = "Precision";

	[CustomizationFieldOption(Type = typeof(int?))]
	public const string RoundingPrecision = "RoundingPrecision";

	[CustomizationFieldOption(Type = typeof(bool?))]
	public const string AllowNulls = "AllowNulls";

	[CustomizationFieldOption(Type = typeof(int?))]
	public const string DropDownWidth = "DropDownWidth";

	public const string Casing = "Casing";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string CheckText = "CheckText";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string RegExpression = "RegExpression";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string Format = "Format";

	[CustomizationFieldOption(Type = typeof(int?))]
	public const string FieldHeight = "FieldHeight";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string RequiredText = "RequiredText";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string Url = "Url";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string Headers = "Headers";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string KeyProperty = "KeyProperty";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string ValueProperty = "ValueProperty";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string ImageUrlProperty = "ImageUrlProperty";

	[CustomizationFieldOption(Type = typeof(bool?))]
	public const string ShowHorizontally = "ShowHorizontally";

	[CustomizationFieldOption(Type = typeof(int?))]
	public const string MaxCountPerRow = "MaxCountPerRow";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string LinkName = "LinkName";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string LinkUrl = "LinkUrl";

	public const string DirectoryPropertyBanks = "Banks";

	public const string DirectoryPropertyCountries = "Countries";

	public const string DirectoryPropertyBranches = "Branches";

	public const string DirectoryPropertyCurrencies = "Currencies";

	[CustomizationFieldOption(Type = typeof(List<KeyValuePair<string, string>>))]
	public const string DataSource = "DataSource";

	[CustomizationFieldOption(Type = typeof(long?))]
	public const string MaxFileSize = "MaxFileSize";

	[CustomizationFieldOption(Type = typeof(List<string>))]
	public const string SupportedFileExtensions = "SupportedFileExtensions";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string InvalidValueErrorText = "InvalidValueErrorText";

	public const string ImageUrl = "ImageUrl";

	[CustomizationFieldOption(Type = typeof(bool?))]
	public const string IsSlider = "IsSlider";

	[CustomizationFieldOption(Type = typeof(decimal?))]
	public const string StepValue = "StepValue";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string UnitName = "UnitName";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string Currency = "Currency";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string CommissionCurrency = "CommissionCurrency";

	[CustomizationFieldOption(Type = typeof(decimal?))]
	public const string Commission = "Commission";

	[CustomizationFieldOption(Type = typeof(string))]
	public const string CommissionText = "CommissionText";

	[CustomizationFieldOption(Type = typeof(bool?))]
	public const string ShowCommission = "ShowCommission";

	public static Type GetOptionType(string option)
	{
		FieldInfo fieldInfo = typeof(CustomizationPropertyName).GetFields(BindingFlags.Static | BindingFlags.Public).FirstOrDefault((FieldInfo x) => ((string)x.GetValue(null)).ToLower() == option.ToLower());
		if (fieldInfo == null)
		{
			return typeof(object);
		}
		CustomizationFieldOptionAttribute customAttribute = fieldInfo.GetCustomAttribute<CustomizationFieldOptionAttribute>();
		if (customAttribute == null)
		{
			return typeof(object);
		}
		return customAttribute.Type;
	}
}
