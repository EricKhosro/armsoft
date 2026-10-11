using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace ArmSoft.SME.Core.UI.Extensions;

public class ThemeBasedNotBrushedColorExtension(string lightThemeColor, string darkThemeColor) : MarkupExtension
{
	public Color LightThemeColor { get; set; } = (Color)ColorConverter.ConvertFromString(lightThemeColor);

	public Color DarkThemeColor { get; set; } = (Color)ColorConverter.ConvertFromString(darkThemeColor);

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (Settings.IsDarkTheme)
		{
			return DarkThemeColor;
		}
		return LightThemeColor;
	}
}
