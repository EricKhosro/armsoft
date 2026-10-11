using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Directories;

public class DirectoryDeleteSelectedResponseModel
{
	public int SuccessCount { get; set; }

	public List<DirectoryDeleteResponseModel> Exceptions { get; set; }
}
