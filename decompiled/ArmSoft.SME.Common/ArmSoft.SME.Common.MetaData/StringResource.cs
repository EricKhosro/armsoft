using System.Collections.Generic;

namespace ArmSoft.SME.Common.MetaData;

public class StringResource
{
	public string Name { get; set; }

	public bool System { get; set; }

	public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
}
