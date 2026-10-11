namespace ArmSoft.SME.Client.Data;

public class ExecutionSettings
{
	public int? QueryTimeOut { get; set; }

	public int? MaxRowCount { get; set; }

	public bool ShowProgress { get; set; }

	public bool ShowColumns { get; set; }

	public RowsTarget? RowsTarget { get; set; }

	public int FetchSize { get; set; }

	public int FirstFetchSize { get; set; }

	public bool SuppressPostProcessing { get; set; }

	public bool SnapshotMode { get; }

	public bool UseRowsBinarySerializer { get; set; }

	public bool ReloadData { get; set; }
}
