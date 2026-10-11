using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core.ConditionalFormatting.Themes;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.HelpClasses;

public static class ConditionalFormattingHelper
{
	public static List<FormatConditionBase> ConvertConditions(List<AsFormatting> formatConditionsList)
	{
		List<FormatConditionBase> list = new List<FormatConditionBase>();
		foreach (AsFormatting formatConditions in formatConditionsList)
		{
			if (formatConditions is AsCondititionFormat)
			{
				FormatCondition item = CreateColorCondition(formatConditions);
				list.Add(item);
			}
			else if (formatConditions is AsTopBottomRuleFormat asTopBottomRuleFormat)
			{
				if (asTopBottomRuleFormat.InnerTopBottomRule() == AsTopBottomRule.Duplicate || asTopBottomRuleFormat.InnerTopBottomRule() == AsTopBottomRule.Unique)
				{
					list.Add(CreateUniqueDuplicateCondition(asTopBottomRuleFormat));
				}
				else
				{
					list.Add(CreateTopBottomCondition(asTopBottomRuleFormat));
				}
			}
			else if (formatConditions is AsDataBarFormat condition)
			{
				list.Add(CreateDataBarCondition(condition));
			}
			else if (formatConditions is AsIconSetFormat condition2)
			{
				list.Add(CreateIconSetCondition(condition2));
			}
		}
		return list;
	}

	private static DataBarFormatCondition CreateDataBarCondition(AsDataBarFormat condition)
	{
		DataBarFormatCondition dataBarFormatCondition = new DataBarFormatCondition();
		if (!string.IsNullOrWhiteSpace(condition.FieldName))
		{
			dataBarFormatCondition.FieldName = condition.FieldName;
		}
		dataBarFormatCondition.PredefinedFormatName = Enum.GetName(typeof(AsPredefinedDataBarFormatNames), condition.InnerFormatName());
		if (!string.IsNullOrWhiteSpace(condition.Expression))
		{
			dataBarFormatCondition.Expression = condition.Expression;
		}
		if (!string.IsNullOrWhiteSpace(condition.SelectiveExpression))
		{
			dataBarFormatCondition.SelectiveExpression = condition.SelectiveExpression;
		}
		if (condition.IsFixed.HasValue && condition.IsFixed.Value)
		{
			dataBarFormatCondition.Fixed = true;
		}
		return dataBarFormatCondition;
	}

	private static IconSetFormatCondition CreateIconSetCondition(AsIconSetFormat condition)
	{
		IconSetFormatCondition iconSetFormatCondition = new IconSetFormatCondition();
		if (!string.IsNullOrWhiteSpace(condition.FieldName))
		{
			iconSetFormatCondition.FieldName = condition.FieldName;
		}
		iconSetFormatCondition.PredefinedFormatName = Enum.GetName(typeof(AsIconSetNames), condition.InnerIconSetName());
		if (!string.IsNullOrWhiteSpace(condition.Expression))
		{
			iconSetFormatCondition.Expression = condition.Expression;
		}
		if (!string.IsNullOrWhiteSpace(condition.SelectiveExpression))
		{
			iconSetFormatCondition.SelectiveExpression = condition.SelectiveExpression;
		}
		if (condition.IsFixed.HasValue && condition.IsFixed.Value)
		{
			iconSetFormatCondition.Fixed = true;
		}
		return iconSetFormatCondition;
	}

	private static FormatCondition CreateColorCondition(AsFormatting condition)
	{
		AsCondititionFormat asCondititionFormat = (AsCondititionFormat)condition;
		FormatCondition formatCondition = new FormatCondition();
		if (!string.IsNullOrWhiteSpace(asCondititionFormat.Expression))
		{
			formatCondition.Expression = asCondititionFormat.Expression;
		}
		if (!string.IsNullOrWhiteSpace(asCondititionFormat.FieldName))
		{
			formatCondition.FieldName = asCondititionFormat.FieldName;
		}
		formatCondition.ApplyToRow = asCondititionFormat.ApplyToRow;
		if (asCondititionFormat.InnerFormatName() == PredefinedFormatNames.Custom && asCondititionFormat.CustomFormat != null)
		{
			formatCondition.Format = CreateDevExFormat(asCondititionFormat.CustomFormat);
		}
		else
		{
			formatCondition.PredefinedFormatName = Enum.GetName(typeof(PredefinedFormatNames), asCondititionFormat.InnerFormatName());
		}
		if (asCondititionFormat.IsFixed.HasValue && asCondititionFormat.IsFixed.Value)
		{
			formatCondition.Fixed = true;
		}
		return formatCondition;
	}

	private static TopBottomRuleFormatCondition CreateTopBottomCondition(AsTopBottomRuleFormat condition)
	{
		TopBottomRuleFormatCondition topBottomRuleFormatCondition = new TopBottomRuleFormatCondition();
		if (!string.IsNullOrWhiteSpace(condition.Expression))
		{
			topBottomRuleFormatCondition.Expression = condition.Expression;
		}
		if (!string.IsNullOrWhiteSpace(condition.FieldName))
		{
			topBottomRuleFormatCondition.FieldName = condition.FieldName;
		}
		switch (condition.InnerTopBottomRule())
		{
		case AsTopBottomRule.AboveAverage:
			topBottomRuleFormatCondition.Rule = TopBottomRule.AboveAverage;
			break;
		case AsTopBottomRule.BelowAverage:
			topBottomRuleFormatCondition.Rule = TopBottomRule.BelowAverage;
			break;
		case AsTopBottomRule.BottomItems:
			topBottomRuleFormatCondition.Rule = TopBottomRule.BottomItems;
			topBottomRuleFormatCondition.Threshold = condition.CountOrPercent;
			break;
		case AsTopBottomRule.BottomPercent:
			topBottomRuleFormatCondition.Rule = TopBottomRule.BottomPercent;
			topBottomRuleFormatCondition.Threshold = condition.CountOrPercent;
			break;
		case AsTopBottomRule.TopItems:
			topBottomRuleFormatCondition.Rule = TopBottomRule.TopItems;
			topBottomRuleFormatCondition.Threshold = condition.CountOrPercent;
			break;
		case AsTopBottomRule.TopPercent:
			topBottomRuleFormatCondition.Rule = TopBottomRule.TopPercent;
			topBottomRuleFormatCondition.Threshold = condition.CountOrPercent;
			break;
		}
		if (condition.InnerFormatName() == PredefinedFormatNames.Custom && condition.CustomFormat != null)
		{
			topBottomRuleFormatCondition.Format = CreateDevExFormat(condition.CustomFormat);
		}
		else
		{
			topBottomRuleFormatCondition.PredefinedFormatName = Enum.GetName(typeof(PredefinedFormatNames), condition.InnerFormatName());
		}
		return topBottomRuleFormatCondition;
	}

	private static UniqueDuplicateRuleFormatCondition CreateUniqueDuplicateCondition(AsTopBottomRuleFormat condition)
	{
		UniqueDuplicateRuleFormatCondition uniqueDuplicateRuleFormatCondition = new UniqueDuplicateRuleFormatCondition();
		if (!string.IsNullOrWhiteSpace(condition.FieldName))
		{
			uniqueDuplicateRuleFormatCondition.FieldName = condition.FieldName;
		}
		if (condition.InnerTopBottomRule() == AsTopBottomRule.Unique)
		{
			uniqueDuplicateRuleFormatCondition.Rule = UniqueDuplicateRule.Unique;
		}
		else
		{
			uniqueDuplicateRuleFormatCondition.Rule = UniqueDuplicateRule.Duplicate;
		}
		if (condition.InnerFormatName() == PredefinedFormatNames.Custom && condition.CustomFormat != null)
		{
			uniqueDuplicateRuleFormatCondition.Format = CreateDevExFormat(condition.CustomFormat);
		}
		else
		{
			uniqueDuplicateRuleFormatCondition.PredefinedFormatName = Enum.GetName(typeof(PredefinedFormatNames), condition.InnerFormatName());
		}
		return uniqueDuplicateRuleFormatCondition;
	}

	public static Format CreateDevExFormat(UserDefinedFormat customFormat)
	{
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		Format format = new Format();
		if (!string.IsNullOrWhiteSpace(customFormat.BackgroundColor))
		{
			format.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(customFormat.BackgroundColor));
		}
		if (!string.IsNullOrWhiteSpace(customFormat.ForegroundColor))
		{
			format.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(customFormat.ForegroundColor));
		}
		if (customFormat.FontSize != -1.0)
		{
			format.FontSize = customFormat.FontSize;
		}
		if (!string.IsNullOrWhiteSpace(customFormat.FontFamily))
		{
			format.FontFamily = (FontFamily)new FontFamilyConverter().ConvertFromString(customFormat.FontFamily);
		}
		if (customFormat.FontIsBold)
		{
			format.FontWeight = FontWeights.Bold;
		}
		if (customFormat.InnerFontStyle() == AsFontStyle.Italic)
		{
			format.FontStyle = FontStyles.Italic;
		}
		if (!string.IsNullOrWhiteSpace(customFormat.IconName))
		{
			ConditionalFormattingIconSetIconExtension conditionalFormattingIconSetIconExtension = new ConditionalFormattingIconSetIconExtension
			{
				IconName = customFormat.IconName
			};
			format.Icon = (ImageSource)conditionalFormattingIconSetIconExtension.ProvideValue(null);
		}
		if (!string.IsNullOrWhiteSpace(customFormat.IconUri))
		{
			format.Icon = ImageService.GetImage(customFormat.IconUri.ToString().Replace("pack://application:,,,/ArmSoft.SME.Core.UI;component/Images/", ""));
		}
		return format;
	}
}
