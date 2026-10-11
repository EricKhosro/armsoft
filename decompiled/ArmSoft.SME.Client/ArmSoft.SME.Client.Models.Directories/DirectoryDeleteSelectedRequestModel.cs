using System.Collections.Generic;

namespace ArmSoft.SME.Client.Models.Directories;

public class DirectoryDeleteSelectedRequestModel
{
	public List<int> Ids { get; set; }

	public List<string> Keys { get; set; }

	public bool ValidateDelete { get; set; }
}
