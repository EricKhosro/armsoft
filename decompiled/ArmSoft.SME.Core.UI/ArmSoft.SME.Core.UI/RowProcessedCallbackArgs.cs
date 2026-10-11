namespace ArmSoft.SME.Core.UI;

public class RowProcessedCallbackArgs
{
	public int Count { get; set; }

	public int TotalCount { get; set; }

	public string ProcessingPhase { get; set; }

	public bool Cancel { get; set; }

	public object Rows { get; set; }

	public RowProcessedCallbackArgs()
	{
		Count = 0;
		TotalCount = 0;
		ProcessingPhase = null;
		Cancel = false;
		Rows = null;
	}
}
