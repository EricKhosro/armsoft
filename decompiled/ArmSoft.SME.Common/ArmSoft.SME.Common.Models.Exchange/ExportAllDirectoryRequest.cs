namespace ArmSoft.SME.Common.Models.Exchange;

public class ExportAllDirectoryRequest
{
	public short DirType { get; set; }

	public ExportAllDirectoryRequest()
	{
	}

	public ExportAllDirectoryRequest(short dirType)
	{
		DirType = dirType;
	}
}
