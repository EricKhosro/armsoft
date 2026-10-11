using System.Windows;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

namespace ArmSoft.SME.Core.UI.MetaData.UI_Definition.DataItems;

public class UINumberDataItem : UIDataItem
{
	public override NumberDataItem Item => base.Item as NumberDataItem;

	public UINumberDataItem(DataItem item)
		: base(item)
	{
	}

	protected override bool ShowCustomAttributeDialog(Window owner = null)
	{
		return new NumberDataItemDialogWindow(Item, owner).ShowDialog().Value;
	}
}
