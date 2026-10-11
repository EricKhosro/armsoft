using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentDeleteSelectedRequestModel : DocumentDeleteRequestModel
{
	public List<Guid> DocISNs { get; set; }
}
