using System;

namespace ArmSoft.SME.Core.UI;

public class UserDefinedFormat
{
	public string BackgroundColor { get; set; }

	public string ForegroundColor { get; set; }

	public string FontFamily { get; set; }

	public double FontSize { get; set; }

	public string FontStyle { get; set; }

	public string IconName { get; set; }

	public string IconUri { get; set; }

	public bool FontIsBold { get; set; }

	public AsFontStyle InnerFontStyle()
	{
		if (FontStyle != null && FontStyle != "")
		{
			return (AsFontStyle)Enum.Parse(typeof(AsFontStyle), FontStyle);
		}
		return AsFontStyle.Normal;
	}

	public UserDefinedFormat()
	{
		BackgroundColor = "";
		ForegroundColor = "";
		FontSize = -1.0;
		FontFamily = "";
		FontIsBold = false;
		FontStyle = "";
		IconName = "";
		IconUri = "";
	}
}
