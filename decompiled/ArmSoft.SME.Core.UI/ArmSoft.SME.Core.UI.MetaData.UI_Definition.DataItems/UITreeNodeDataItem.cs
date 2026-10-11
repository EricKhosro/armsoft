using System.Windows;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

namespace ArmSoft.SME.Core.UI.MetaData.UI_Definition.DataItems;

public class UITreeNodeDataItem : UIDataItem
{
	public override TreeNodeDataItem Item => base.Item as TreeNodeDataItem;

	public UITreeNodeDataItem(DataItem item)
		: base(item)
	{
	}

	protected override bool ShowCustomAttributeDialog(Window owner = null)
	{
		return new TreeNodeDataItemDialogWindow(Item, owner).ShowDialog().Value;
	}
}
