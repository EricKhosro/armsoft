using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentChangeStateRequestModel
{
	public List<Guid> DocISNs { get; set; }

	public byte State { get; set; }

	public bool RegenerateEntries { get; set; }
}
