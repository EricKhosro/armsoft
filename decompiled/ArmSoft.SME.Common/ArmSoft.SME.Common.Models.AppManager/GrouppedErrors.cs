using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.AppManager;

public class GrouppedErrors
{
	public string Header { get; set; }

	public List<string> Errors { get; set; } = new List<string>();

	public GrouppedErrors()
	{
	}

	public GrouppedErrors(string header)
	{
		Header = header;
	}
}
