using System.Windows;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData.UI_Definition.DataItems;

public abstract class UIDataItem
{
	internal bool IsConfigured { get; set; }

	public virtual DataItem Item { get; }

	public UIDataItem(DataItem item)
	{
		Item = item;
	}

	protected abstract bool ShowCustomAttributeDialog(Window owner = null);

	public bool ShowAttributeDialog(Window owner = null)
	{
		bool flag = ShowCustomAttributeDialog(owner);
		if (flag)
		{
			IsConfigured = true;
		}
		return flag;
	}
}
