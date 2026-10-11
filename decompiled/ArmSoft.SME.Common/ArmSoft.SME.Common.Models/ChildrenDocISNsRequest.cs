using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models;

public class ChildrenDocISNsRequest
{
	public List<Guid> DocISNs { get; set; }

	public List<short> ChildDocTypes { get; set; }
}
