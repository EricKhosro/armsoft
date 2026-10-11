using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Data;

public class DataViewSetting
{
	public short Id { get; set; }

	public string ViewName { get; set; }

	public string GroupName { get; set; }

	public string Caption { get; set; }

	public string Definition { get; set; }

	public string FilterString { get; set; }

	public List<string> Columns { get; set; } = new List<string>();

	public List<ColumnOrder> SortOrder { get; set; } = new List<ColumnOrder>();

	public List<List<string>> GroupBy { get; set; } = new List<List<string>>();

	public void Clear()
	{
		Columns.Clear();
		SortOrder.Clear();
		GroupBy.Clear();
		FilterString = "";
	}

	public DataViewSetting Clone()
	{
		DataViewSetting dataViewSetting = new DataViewSetting
		{
			ViewName = ViewName,
			GroupName = GroupName,
			Caption = Caption
		};
		foreach (string column in Columns)
		{
			dataViewSetting.Columns.Add(column);
		}
		foreach (ColumnOrder item in SortOrder)
		{
			dataViewSetting.SortOrder.Add(item);
		}
		foreach (List<string> item2 in GroupBy)
		{
			List<string> list = new List<string>();
			foreach (string item3 in item2)
			{
				list.Add(item3);
			}
			dataViewSetting.GroupBy.Add(list);
		}
		dataViewSetting.FilterString = FilterString;
		return dataViewSetting;
	}
}
