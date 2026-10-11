using System;

namespace ArmSoft.SME.Core.UI;

public class AsTopBottomRuleFormat : AsFormatting
{
	public string FieldName { get; set; }

	public string FormatName { get; set; }

	public string Expression { get; set; }

	public UserDefinedFormat CustomFormat { get; set; }

	public string TopBottomRule { get; set; }

	public int CountOrPercent { get; set; }

	public PredefinedFormatNames InnerFormatName()
	{
		if (FormatName != null && FormatName != "")
		{
			return (PredefinedFormatNames)Enum.Parse(typeof(PredefinedFormatNames), FormatName);
		}
		return PredefinedFormatNames.Custom;
	}

	public AsTopBottomRule InnerTopBottomRule()
	{
		if (TopBottomRule != null && TopBottomRule != "")
		{
			return (AsTopBottomRule)Enum.Parse(typeof(AsTopBottomRule), TopBottomRule);
		}
		return AsTopBottomRule.TopItems;
	}

	public AsTopBottomRuleFormat()
	{
		FieldName = "";
		TopBottomRule = "TopItems";
		CountOrPercent = 10;
		Expression = "";
	}
}
