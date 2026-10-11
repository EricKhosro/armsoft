using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.MetaData.UI_Definition.DataItems;

namespace ArmSoft.SME.Core.UI.MetaData;

internal static class DataItemHelper
{
	private static List<string> _dataItemHorizontalAlignmentList;

	internal static List<string> DataItemHorizontalAlignmentList
	{
		get
		{
			if (_dataItemHorizontalAlignmentList == null)
			{
				int num = 4;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount(list, num);
				Span<string> span = CollectionsMarshal.AsSpan(list);
				span[0] = HorizontalAlignment.Left.ToString();
				span[1] = HorizontalAlignment.Right.ToString();
				span[2] = HorizontalAlignment.Center.ToString();
				span[3] = HorizontalAlignment.Stretch.ToString();
				_dataItemHorizontalAlignmentList = list;
			}
			return _dataItemHorizontalAlignmentList;
		}
	}

	internal static DataItem GetDataItem(string type)
	{
		return type switch
		{
			"Boolean" => (DataItem)new BooleanDataItem(), 
			"Date" => new DateDataItem(), 
			"Number" => new NumberDataItem(), 
			"String" => new StringDataItem(), 
			"Time" => new TimeDataItem(), 
			"TreeNode" => new TreeNodeDataItem(), 
			_ => null, 
		};
	}

	internal static UIDataItem GetUIDataItem(this DataItem item)
	{
		if (item == null)
		{
			return null;
		}
		return item.Type switch
		{
			"Boolean" => (UIDataItem)new UIBooleanDataItem(item), 
			"Date" => new UIDateDataItem(item), 
			"Number" => new UINumberDataItem(item), 
			"String" => new UIStringDataItem(item), 
			"Time" => new UITimeDataItem(item), 
			"TreeNode" => new UITreeNodeDataItem(item), 
			_ => null, 
		};
	}
}
