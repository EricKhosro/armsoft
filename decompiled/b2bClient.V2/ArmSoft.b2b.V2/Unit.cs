using System.Collections.Generic;

namespace ArmSoft.b2b.V2;

public class Unit : IdName
{
	public bool HasFraction { get; set; }

	public bool IsDefault { get; set; }

	public List<string> BarCodes { get; set; }
}
