using System.Collections.Generic;

namespace ArmSoft.OpenBanking.Api.Common;

public class Merchant
{
	public string Id { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public List<Terminal> Terminals { get; set; } = new List<Terminal>();
}
