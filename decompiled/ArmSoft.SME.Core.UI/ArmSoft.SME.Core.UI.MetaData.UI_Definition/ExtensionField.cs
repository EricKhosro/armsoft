namespace ArmSoft.SME.Core.UI.MetaData.UI_Definition;

public class ExtensionField : FieldDescription
{
	public string Tab { get; set; }

	public string Group { get; set; }

	public string Mask => "UD_.*";

	public ExtensionField(UIField field = null, string tabCaption = null, string groupCaption = null)
	{
		if (field != null)
		{
			Name = field.Name;
			CaptionId = ((!string.IsNullOrWhiteSpace(field.CaptionId)) ? field.CaptionId : field.Caption);
			Type = field.DataItem.Type;
			UIDataItem = field.DataItem.GetUIDataItem();
			UIDataItem.IsConfigured = true;
			DropDownItem = field.DropDownItem;
			DropDownType = field.DropDownItem?.Type;
			Tab = tabCaption;
			Group = groupCaption;
			HideCaption = field.HideCaption;
			HorizontalAlignment = field.HorizontalAlignment;
			ShowInMobile = field.ShowInMobile;
			StoreValue = field.StoreValue;
		}
	}
}
