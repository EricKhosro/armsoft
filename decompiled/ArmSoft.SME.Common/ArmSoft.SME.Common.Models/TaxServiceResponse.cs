using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models;

public class TaxServiceResponse
{
	public uint AssignedCount { get; set; }

	public uint NotAssignedCount { get; set; }

	public List<TaxServiceItems> Items { get; set; }
}
