using System;

namespace ArmSoft.SME.Core.UI;

public class AsIconSetFormat : AsFormatting
{
	public string FieldName { get; set; }

	public string IconSetName { get; set; }

	public string Expression { get; set; }

	public string SelectiveExpression { get; set; }

	public AsIconSetNames InnerIconSetName()
	{
		if (IconSetName != null && IconSetName != "")
		{
			return (AsIconSetNames)Enum.Parse(typeof(AsIconSetNames), IconSetName);
		}
		return AsIconSetNames.TrafficLights3UnrimmedIconSet;
	}

	public AsIconSetFormat()
	{
		FieldName = "";
		IconSetName = "TrafficLights3UnrimmedIconSet";
		Expression = "";
		SelectiveExpression = "";
	}
}
