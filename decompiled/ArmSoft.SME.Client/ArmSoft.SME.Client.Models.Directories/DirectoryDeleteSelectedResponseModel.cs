using System.Collections.Generic;

namespace ArmSoft.SME.Client.Models.Directories;

public class DirectoryDeleteSelectedResponseModel
{
	public List<DirectoryDeleteResponseModel> Exceptions { get; set; }
}
