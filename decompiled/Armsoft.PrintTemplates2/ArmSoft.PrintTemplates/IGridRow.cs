namespace ArmSoft.PrintTemplates;

public interface IGridRow
{
	object this[int index] { get; set; }

	object this[string columnName] { get; set; }

	internal object[] Row { get; }

	internal string GetFormattedValue(GridColumn gridColumn);

	internal void ConvertToArmenianUnicode();
}
