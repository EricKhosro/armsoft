using System.ComponentModel;

namespace ArmSoft.SME.Core.UI.DataViews;

public class SerializableGroupingInfo
{
	public bool ShowQty { get; set; }

	public BindingList<GroupingColumn> ColumnsInfo { get; set; }

	public string Name { get; set; }
}
