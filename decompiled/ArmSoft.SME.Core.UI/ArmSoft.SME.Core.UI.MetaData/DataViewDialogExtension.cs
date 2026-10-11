using System.Collections.Generic;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public class DataViewDialogExtension
{
	public Dictionary<string, DataItem> DataItems { get; set; }

	public List<UIElement> Elements { get; set; }

	public Dictionary<string, object> Values { get; set; }

	public bool HasCalculatedDataItems
	{
		get
		{
			if (DataItems == null)
			{
				return false;
			}
			foreach (DataItem value in DataItems.Values)
			{
				if (value.IsCalculated)
				{
					return true;
				}
			}
			return false;
		}
	}
}
