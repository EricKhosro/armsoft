using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Directories;

namespace ArmSoft.SME.Client.Models.Directories;

public class DirectoryStoreSelectedRequestModel<T> where T : Directory, new()
{
	public DirStoreRequest StoreRequest { get; set; }

	public List<T> Directory { get; set; }
}
