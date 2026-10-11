using ArmSoft.SME.Common.Models.Directories;

namespace ArmSoft.SME.Client.Models.Directories;

public class DirectoryStoreRequestModel<T> where T : Directory, new()
{
	public bool ReturnDocument { get; set; } = true;

	public DirStoreRequest StoreRequest { get; set; }

	public T Directory { get; set; }

	public bool ThrowException { get; set; }
}
