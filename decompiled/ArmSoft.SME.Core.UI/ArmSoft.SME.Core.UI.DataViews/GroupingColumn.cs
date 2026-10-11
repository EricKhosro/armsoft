using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Core.UI.DataViews;

public class GroupingColumn
{
	public int Len;

	public int PrecisionLen = -1;

	public string Name { get; set; }

	public bool GroupBy { get; set; }

	public bool CalcMax { get; set; }

	public bool CalcMin { get; set; }

	public bool CalcSum { get; set; }

	public bool CalcAvg { get; set; }

	public string Caption { get; set; }

	public DataColumnType TypeGroup { get; set; } = DataColumnType.Unknown;

	public bool AllowSum { get; set; }

	public bool EnableAggregates { get; set; }

	public bool UsedInGrouping
	{
		get
		{
			if (!GroupBy && !CalcSum && !CalcAvg && !CalcMax)
			{
				return CalcMin;
			}
			return true;
		}
	}

	public GroupingColumn()
	{
		Name = "";
	}
}
