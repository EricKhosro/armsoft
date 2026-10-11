using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Docking;

namespace ArmSoft.SME.Core.UI.Helpers;

public static class ASFontHelper
{
	public static void SetFontSettings(UserControl userControl)
	{
		userControl.FontFamily = Settings.FontFamily;
		userControl.FontSize = Settings.GetFontSize();
	}

	public static void SetFontSettings(Window window)
	{
		window.FontFamily = Settings.FontFamily;
		window.FontSize = Settings.GetFontSize();
	}

	public static void SetFontSettings(DocumentPanel documentPanel)
	{
		documentPanel.FontFamily = Settings.FontFamily;
		documentPanel.FontSize = Settings.GetFontSize();
	}

	public static void SetFontSettings(LayoutPanel layoutPanel)
	{
		layoutPanel.FontFamily = Settings.FontFamily;
		layoutPanel.FontSize = Settings.GetFontSize();
	}
}
