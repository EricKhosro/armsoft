using System.Windows;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

namespace ArmSoft.SME.Core.UI.MetaData.UI_Definition.DataItems;

public class UIStringDataItem : UIDataItem
{
	public override StringDataItem Item => base.Item as StringDataItem;

	public UIStringDataItem(DataItem item)
		: base(item)
	{
	}

	protected override bool ShowCustomAttributeDialog(Window owner = null)
	{
		return new StringDataItemDialogWindow(Item, owner).ShowDialog().Value;
	}
}
