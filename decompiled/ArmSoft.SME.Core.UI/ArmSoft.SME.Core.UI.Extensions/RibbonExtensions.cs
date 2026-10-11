using System.Windows;
using DevExpress.Xpf.Bars;

namespace ArmSoft.SME.Core.UI.Extensions;

public static class RibbonExtensions
{
	public static void AddWithMergingProperties(this BarItemCollection items, BarItem item, string name)
	{
		MergingProperties.SetName((DependencyObject)(object)item, name);
		items.Add(item);
	}

	public static void AddWithName(this BarItemCollection items, BarItem item, string name)
	{
		item.Name = name;
		items.Add(item);
	}

	public static BarButtonItemLink ConvertForToolbar(this BarItem item, string glyph, bool usePalette = true)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		if (Settings.IsWin11Theme)
		{
			return (BarButtonItemLink)item.CreateLink();
		}
		BarSubItem barSubItem = item as BarSubItem;
		if (barSubItem != null)
		{
			BarSubItem barSubItem2 = new BarSubItem
			{
				Content = item.Content,
				KeyGesture = item.KeyGesture
			};
			bool usePalette2 = usePalette;
			barSubItem2.Glyph = ImageService.GetImage(glyph, isAppPathPreferred: false, null, default, usePalette2);
			barSubItem2.Name = item.Name;
			barSubItem2.IsVisible = item.IsVisible;
			barSubItem2.IsEnabled = item.IsEnabled;
			barSubItem2.ToolTip = item.ToolTip;
			BarSubItem barSubItem3 = barSubItem2;
			foreach (BarItemLinkBase itemLink in barSubItem.ItemLinks)
			{
				barSubItem3.ItemLinks.Add(itemLink);
			}
			barSubItem3.ItemClick += (object s, ItemClickEventArgs e) =>
			{
				CloneItem_ItemClick(barSubItem);
			};
			return (BarButtonItemLink)barSubItem3.CreateLink();
		}
		BarButtonItem barButtonItem = item as BarButtonItem;
		if (barButtonItem != null)
		{
			BarButtonItem barButtonItem2 = new BarButtonItem
			{
				Content = item.Content,
				KeyGesture = item.KeyGesture
			};
			bool usePalette2 = usePalette;
			barButtonItem2.Glyph = ImageService.GetImage(glyph, isAppPathPreferred: false, null, default, usePalette2);
			barButtonItem2.Name = item.Name;
			barButtonItem2.IsVisible = item.IsVisible;
			barButtonItem2.IsEnabled = item.IsEnabled;
			barButtonItem2.ToolTip = item.ToolTip;
			barButtonItem2.SuperTip = item.SuperTip;
			barButtonItem2.ItemClick += (object s, ItemClickEventArgs e) =>
			{
				CloneItem_ItemClick(barButtonItem);
			};
			return (BarButtonItemLink)barButtonItem2.CreateLink();
		}
		throw new ASException("Invalid bar item type");
	}

	private static void CloneItem_ItemClick(BarButtonItem item)
	{
		item.PerformClick();
	}
}
