using System.Windows;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

namespace ArmSoft.SME.Core.UI.MetaData.UI_Definition.DataItems;

public class UIBooleanDataItem : UIDataItem
{
	public override BooleanDataItem Item => base.Item as BooleanDataItem;

	public UIBooleanDataItem(DataItem item)
		: base(item)
	{
		IsConfigured = true;
	}

	protected override bool ShowCustomAttributeDialog(Window owner = null)
	{
		return new BooleanDataItemDialogWindow(Item, owner).ShowDialog().Value;
	}
}
