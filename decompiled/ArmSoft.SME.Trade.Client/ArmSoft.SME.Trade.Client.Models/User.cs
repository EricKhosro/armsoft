using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Models;

public class User : ExtendableRow
{
	public string Name { get; set; }

	public string Description { get; set; }

	public bool Login { get; set; }

	public bool Admin { get; set; }

	public int? LocalUserId { get; set; }

	public int CloudUserId { get; set; }
}
