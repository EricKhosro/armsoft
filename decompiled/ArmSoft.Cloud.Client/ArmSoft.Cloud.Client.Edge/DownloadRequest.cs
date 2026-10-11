namespace ArmSoft.Cloud.Client.Edge;

public class DownloadRequest
{
	public string FileName { get; set; }

	public long Offset { get; set; }

	public int BufferLength { get; set; }
}
