namespace ArmSoft.SME.Common.Models.Directories;

public class DirectoryStoreRequestModel
{
	public bool ReturnDocument { get; set; } = true;

	public DirStoreRequest StoreRequest { get; set; }

	public string JSONDocument { get; set; }

	public bool ThrowException { get; set; }
}
