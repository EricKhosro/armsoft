using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Exchange;

public class DirectoryExchangeRequest
{
	public List<string> Keys { get; set; }

	public List<int> Ids { get; set; }
}
