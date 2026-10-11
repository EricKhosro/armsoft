using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Models.Directories;

public class DirectoryDeleteResponseModel
{
	public int? Id { get; set; }

	public string Key { get; set; }

	public ExceptionModel Exception { get; set; }
}
