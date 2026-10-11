using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Directories;

public class DirectoryStoreSelectedRequestModel
{
	public DirStoreRequest StoreRequest { get; set; }

	public List<string> JSONDocuments { get; set; }
}
