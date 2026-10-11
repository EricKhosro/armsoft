using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Directories;

public class DirectoryEditFieldsWithSameValueResponseModel
{
	public int SuccessCount { get; set; }

	public List<DirectoryEditResponseModel> Exceptions { get; set; }
}
