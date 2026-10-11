namespace ArmSoft.Cloud.Client.Edge;

public class UploadRequest
{
	public string FileName { get; set; }

	public byte[] Buffer { get; set; }

	public long Offset { get; set; }

	public int BytesLength { get; set; }
}
