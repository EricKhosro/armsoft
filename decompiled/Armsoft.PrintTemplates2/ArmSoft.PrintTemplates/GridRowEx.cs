using System.Collections.Generic;

namespace ArmSoft.PrintTemplates;

public sealed class GridRowEx : GridRow<object>
{
	public GridRowEx(Dictionary<string, GridColumn> columns, int columnMaxCount)
		: base(columns, columnMaxCount)
	{
	}
}
