using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Directories;

public class DirectoryStoreResponseModel
{
	public List<ExceptionModel> Exceptions { get; set; }

	public string JSONDocument { get; set; }
}
