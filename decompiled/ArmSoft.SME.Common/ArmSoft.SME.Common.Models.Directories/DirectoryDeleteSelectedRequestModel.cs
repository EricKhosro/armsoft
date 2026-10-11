using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Directories;

public class DirectoryDeleteSelectedRequestModel
{
	public short Type { get; set; }

	public List<int> Ids { get; set; }

	public List<string> Keys { get; set; }

	public bool WithoutValidation { get; set; }
}
