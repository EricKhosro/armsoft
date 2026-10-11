using ArmSoft.SME.Common.Models.Directories;

namespace ArmSoft.SME.Client.Models.Directories;

public class DirectoryStoreRequestModelBase
{
	public bool ReturnDocument { get; set; } = true;

	public DirStoreRequest StoreRequest { get; set; }

	public Directory Directory { get; set; }
}
