using System;

namespace ArmSoft.PrintTemplates;

public class GridColumn : IComparable<GridColumn>
{
	public string Name { get; internal set; }

	public string Caption { get; internal set; }

	public int Index { get; internal set; }

	public Func<object, string> Formatter { get; private set; }

	public GridColumn(string name, string caption, int index, Func<object, string> formtter = null)
	{
		Name = name;
		Caption = caption;
		Index = index;
		Formatter = formtter;
	}

	public int CompareTo(GridColumn other)
	{
		if (other == null || other.Name == null)
		{
			return -1;
		}
		int num = -Name.Length.CompareTo(other.Name.Length);
		if (num != 0)
		{
			return num;
		}
		return Name.CompareTo(other.Name);
	}
}
