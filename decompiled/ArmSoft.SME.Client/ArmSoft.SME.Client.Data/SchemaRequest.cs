namespace ArmSoft.SME.Client.Data;

public class SchemaRequest
{
	public int? QueryTimeOut { get; set; }

	public int? MaxRowCount { get; set; }

	public bool ShowProgress { get; set; }

	public bool ShowColumns { get; set; }

	public RowsTarget? RowsTarget { get; set; }
}
