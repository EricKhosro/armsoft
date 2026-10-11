namespace Armsoft.PrintTemplates2.Data;

public class DataColumnOptions
{
	public string Format { get; set; }

	public bool HasSum { get; set; }

	public DataColumnOptions(string format, bool hasSum = false)
	{
		Format = format;
		HasSum = hasSum;
	}
}
