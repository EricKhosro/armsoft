using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace ArmSoft.SME.Core.UI.Extensions;

public class ThemeBasedColorExtension(string lightThemeColor, string darkThemeColor) : MarkupExtension
{
	public Brush LightThemeColor { get; set; } = (Brush)new BrushConverter().ConvertFromString(lightThemeColor);

	public Brush DarkThemeColor { get; set; } = (Brush)new BrushConverter().ConvertFromString(darkThemeColor);

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (Settings.IsDarkTheme)
		{
			return DarkThemeColor;
		}
		return LightThemeColor;
	}
}
