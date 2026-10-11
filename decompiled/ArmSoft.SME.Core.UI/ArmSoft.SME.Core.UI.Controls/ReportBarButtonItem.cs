using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Ribbon;

namespace ArmSoft.SME.Core.UI.Controls;

public class ReportBarButtonItem : BarButtonItem, ICloneable
{
	private readonly short _dataProviderType;

	private readonly bool _showDialog;

	public List<PropertyWithValue> PropertyWithValues { get; set; }

	public ReportBarButtonItem()
	{
	}

	public ReportBarButtonItem(short dataProviderType, bool showDialog, string content, List<PropertyWithValue> propertyWithValues = null, string glyph = "report_16.svg", string largeGlyph = "report_32.svg")
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		_dataProviderType = dataProviderType;
		PropertyWithValues = propertyWithValues;
		_showDialog = showDialog;
		Content = content;
		Glyph = ImageService.GetImage(glyph);
		LargeGlyph = ImageService.GetImage(largeGlyph);
		ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtons_ItemClick).Handler;
	}

	private void BarButtons_ItemClick(object sender, ItemClickEventArgs e)
	{
		Settings.DataViewService.ShowDataView(_dataProviderType, PropertyWithValues ?? new List<PropertyWithValue>(), _showDialog);
	}

	public void AddInBarSubItem(ref RibbonPageGroup ribbonBarItemLinks, ref BarSubItem barSubItem)
	{
		if (Settings.UserAccessService.GetProviderPermission(_dataProviderType).AllowView)
		{
			IsVisible = true;
			ribbonBarItemLinks.ItemLinks.Add(CreateLink());
			BarItemLink barItemLink = CreateLink();
			barItemLink.MergeType = BarItemMergeType.Replace;
			barSubItem.ItemLinks.Add(barItemLink);
		}
		else
		{
			IsVisible = false;
		}
	}

	public void AddInBarSubItem(ref BarSubItem ribbonBaseBarItemLinks)
	{
		if (Settings.UserAccessService.GetProviderPermission(_dataProviderType).AllowView)
		{
			BarItemLink barItemLink = CreateLink();
			barItemLink.MergeType = BarItemMergeType.Replace;
			ribbonBaseBarItemLinks.ItemLinks.Add(barItemLink);
		}
	}

	public object Clone()
	{
		return new ReportBarButtonItem(_dataProviderType, _showDialog, Content.ToString(), PropertyWithValues);
	}
}
