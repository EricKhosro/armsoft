using System;

namespace ArmSoft.SME.Core.UI;

public class AsCondititionFormat : AsFormatting
{
	public string Expression { get; set; }

	public string FieldName { get; set; }

	public bool ApplyToRow { get; set; }

	public UserDefinedFormat CustomFormat { get; set; }

	public string FormatName { get; set; }

	public PredefinedFormatNames InnerFormatName()
	{
		if (FormatName != null && FormatName != "")
		{
			return (PredefinedFormatNames)Enum.Parse(typeof(PredefinedFormatNames), FormatName);
		}
		return PredefinedFormatNames.Custom;
	}

	public AsCondititionFormat()
	{
		Expression = "";
		FieldName = "";
		ApplyToRow = false;
		CustomFormat = null;
	}
}
