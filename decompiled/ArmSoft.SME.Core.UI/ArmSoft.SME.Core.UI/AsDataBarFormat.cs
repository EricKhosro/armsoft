using System;

namespace ArmSoft.SME.Core.UI;

public class AsDataBarFormat : AsFormatting
{
	public string DataBarFormatName;

	public string FieldName { get; set; }

	public string Expression { get; set; }

	public string SelectiveExpression { get; set; }

	public AsPredefinedDataBarFormatNames InnerFormatName()
	{
		if (DataBarFormatName != null && DataBarFormatName != "")
		{
			return (AsPredefinedDataBarFormatNames)Enum.Parse(typeof(AsPredefinedDataBarFormatNames), DataBarFormatName);
		}
		return AsPredefinedDataBarFormatNames.LightBlueGradientDataBar;
	}

	public AsDataBarFormat()
	{
		FieldName = "";
		DataBarFormatName = "BlueGradientDataBar";
		Expression = "";
		SelectiveExpression = "";
	}
}
