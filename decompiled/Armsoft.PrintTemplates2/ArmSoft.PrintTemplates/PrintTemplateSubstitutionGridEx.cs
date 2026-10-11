namespace ArmSoft.PrintTemplates;

public sealed class PrintTemplateSubstitutionGridEx : PrintTemplateSubstitutionGrid<GridRowEx>
{
	public PrintTemplateSubstitutionGridEx(string name)
		: base(name)
	{
	}

	public R GetValue<R>(int row, string column)
	{
		return InnerGetValue<R>(row, column);
	}

	public object GetValue(int row, string column)
	{
		return GetValue<object>(row, column);
	}
}
