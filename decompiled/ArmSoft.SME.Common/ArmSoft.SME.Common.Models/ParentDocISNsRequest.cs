using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models;

public class ParentDocISNsRequest
{
	public List<Guid> DocISNs { get; set; }

	public List<short> ParentDocTypes { get; set; }
}
