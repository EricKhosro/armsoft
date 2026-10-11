using System.Windows;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

namespace ArmSoft.SME.Core.UI.MetaData.UI_Definition.DataItems;

public class UITimeDataItem : UIDataItem
{
	public override TimeDataItem Item => base.Item as TimeDataItem;

	public UITimeDataItem(DataItem item)
		: base(item)
	{
		IsConfigured = true;
	}

	protected override bool ShowCustomAttributeDialog(Window owner = null)
	{
		return new TimeDataItemDialogWindow(Item, owner).ShowDialog().Value;
	}
}
