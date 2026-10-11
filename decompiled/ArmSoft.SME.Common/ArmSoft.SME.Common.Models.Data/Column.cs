using ArmSoft.SME.Common.Data;

namespace ArmSoft.SME.Common.Models.Data;

public class Column
{
	public string Name { get; set; }

	public string Caption { get; set; }

	public SupportedType Type { get; set; }

	public ColumnSourceType SourceType { get; set; }

	public string Format { get; set; }

	public string Group { get; set; }

	public string Source { get; set; }

	public bool FooterSum { get; set; }

	public string Before { get; set; }

	public string After { get; set; }

	public bool DataOnly { get; set; }
}
