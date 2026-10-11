using System.Collections.Generic;
using System.Windows;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.Controls;

public class BarEditItemExt : BarEditItem
{
	public delegate void RoutedEventHandler(object sender, RoutedEventArgs e);

	public SimpleButton simpleButton;

	public event RoutedEventHandler Click;

	public void AddButton(string imageURI = "", string caption = "", string tooltip = "", double width = 0.0)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		Padding = new Thickness(0.0);
		SimpleButton simpleButton = new SimpleButton
		{
			Glyph = ImageService.GetImage(imageURI),
			Content = caption,
			FontFamily = Settings.FontFamily,
			FontSize = Settings.GetFontSize(),
			ToolTip = tooltip,
			Padding = new Thickness(0.0),
			Margin = new Thickness(0.0, 0.0, 0.0, 2.0),
			IsTabStop = true
		};
		if (width > 0.0)
		{
			simpleButton.Width = width;
		}
		else
		{
			simpleButton.GlyphToContentOffset = 3.0;
			simpleButton.Padding = new Thickness(2.0, 0.0, 2.0, 0.0);
		}
		simpleButton.Click += new SafeEventHandler<RoutedEventArgs>(Btn_Click).Handler;
		Content2 = simpleButton;
	}

	public void AddSplitButton(string imageURI = "", string caption = "", string tooltip = "", bool isTabStop = false, double imgWidth = 16.0, double imgHeight = 16.0, IEnumerable<IBarItem> popupItems = null, bool showGlyphsInPopupMenu = true, double width = 0.0)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		Padding = new Thickness(0.0);
		SplitButton splitButton = new SplitButton
		{
			IsTabStop = isTabStop,
			FontFamily = Settings.FontFamily,
			FontSize = Settings.GetFontSize(),
			Height = 20.0,
			BorderThickness = new Thickness(1.0),
			VerticalContentAlignment = VerticalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Top,
			Margin = new Thickness(0.0),
			Padding = new Thickness(0.0),
			ArrowPadding = new Thickness(2.0, 0.0, 2.0, 0.0),
			Width = width
		};
		if (!string.IsNullOrWhiteSpace(tooltip))
		{
			splitButton.ToolTip = tooltip;
		}
		if (string.IsNullOrWhiteSpace(imageURI))
		{
			splitButton.Content = caption;
		}
		else
		{
			splitButton.Glyph = ImageService.GetImage(imageURI);
			splitButton.GlyphHeight = imgHeight;
			splitButton.GlyphWidth = imgWidth;
		}
		if (popupItems != null)
		{
			PopupMenu popupMenu = new PopupMenu();
			BarManager.SetShowGlyphsInPopupMenus((DependencyObject)(object)popupMenu, showGlyphsInPopupMenu);
			foreach (IBarItem popupItem in popupItems)
			{
				popupMenu.Items.Add(popupItem);
			}
			splitButton.PopupContent = popupMenu;
		}
		splitButton.Click += new SafeEventHandler<RoutedEventArgs>(Btn_Click).Handler;
		Content2 = splitButton;
	}

	public void AddDropDown(LayoutItemWithRequired sp, DropDownControl dropDown, string caption = "", string tooltip = "")
	{
		Padding = new Thickness(0.0);
		sp.Margin = new Thickness(20.0, 0.0, 0.0, 0.0);
		sp.Content = dropDown;
		sp.Label = caption;
		sp.FontFamily = Settings.FontFamily;
		sp.FontSize = Settings.GetFontSize();
		dropDown.ToolTip = tooltip;
		Content2 = sp;
	}

	private void Btn_Click(object sender, RoutedEventArgs e)
	{
		Click?.Invoke(this, e);
		e.Handled = true;
	}
}
