using System;
using ArmSoft.SME.Core.UI.MetaData.UI_Definition.DataItems;

namespace ArmSoft.SME.Core.UI.MetaData.UI_Definition;

public class FieldDescription : UIElementDescription
{
	private UIDataItem _dataItem;

	public string Name { get; set; }

	public string Type { get; set; }

	public UIDataItem UIDataItem
	{
		get
		{
			return _dataItem;
		}
		set
		{
			_dataItem = value;
			_dataItem.Item.Name = Name;
			_dataItem.Item.CaptionId = CaptionId;
		}
	}

	public DropDown DropDownItem { get; set; }

	public string DropDownType { get; set; }

	public HorizontalAlignment HorizontalAlignment { get; set; }

	public string HorizontalAlignmentName
	{
		get
		{
			return HorizontalAlignment.ToString();
		}
		set
		{
			if (Enum.TryParse<HorizontalAlignment>(value, out var result))
			{
				HorizontalAlignment = result;
			}
		}
	}

	public bool HideCaption { get; set; }

	public bool StoreValue { get; set; }

	public bool ShowInMobile { get; set; }
}
