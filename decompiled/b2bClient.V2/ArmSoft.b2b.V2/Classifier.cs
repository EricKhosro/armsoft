using System;

namespace ArmSoft.b2b.V2;

public class Classifier : CodeName
{
	public bool IsService { get; set; }

	public DateTime? SignableFrom { get; set; }

	public DateTime? SignableFromLargeScale { get; set; }
}
