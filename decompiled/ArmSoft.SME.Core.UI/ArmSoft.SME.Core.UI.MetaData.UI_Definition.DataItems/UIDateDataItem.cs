using System.Windows;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

namespace ArmSoft.SME.Core.UI.MetaData.UI_Definition.DataItems;

public class UIDateDataItem : UIDataItem
{
	public override DateDataItem Item => base.Item as DateDataItem;

	public UIDateDataItem(DataItem item)
		: base(item)
	{
		IsConfigured = true;
	}

	protected override bool ShowCustomAttributeDialog(Window owner = null)
	{
		return new DateDataItemDialogWindow(Item, owner).ShowDialog().Value;
	}
}
