namespace ArmSoft.Cloud.Client.Edge;

public class ArchiveComment
{
	public int DbId { get; set; }

	public string DbName { get; set; }

	public string DbVersion { get; set; }

	public long UncompressedSize { get; set; }

	public string Comment { get; set; }

	public int UserId { get; set; }

	public string UserName { get; set; }
}
