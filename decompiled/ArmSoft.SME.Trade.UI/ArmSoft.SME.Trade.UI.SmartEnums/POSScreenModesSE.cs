using System;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.SmartEnums;

[SmartEnumProviderSupport(1, "Type")]
public class POSScreenModesSE : ASSmartEnum<POSScreenModesSE, string>
{
	public static readonly POSScreenModesSE Keyboard = new POSScreenModesSE("Keyboard", "1", () => Resources.KeyboardMode, showInDropDown: false);

	public static readonly POSScreenModesSE TouchScreen = new POSScreenModesSE("TouchScreen", "2", () => Resources.TouchScreenMode, showInDropDown: true);

	public static readonly POSScreenModesSE TouchScreenWithFavorites = new POSScreenModesSE("TouchScreenWithFavorites", "3", () => Resources.TouchScreenWithFavoritesMode, showInDropDown: true);

	public static readonly POSScreenModesSE SampleScreen = new POSScreenModesSE("SampleScreen", "4", () => Resources.SampleScreenMode, showInDropDown: true);

	public static readonly POSScreenModesSE SampleScreenWithFavorites = new POSScreenModesSE("SampleScreenWithFavorites", "5", () => Resources.SampleScreenWithFavoritesMode, showInDropDown: true);

	public bool ShowInDropDown;

	public POSScreenModesSE(string name, string value, Func<string> caption, bool showInDropDown)
		: base(name, value, caption)
	{
		ShowInDropDown = showInDropDown;
	}
}
