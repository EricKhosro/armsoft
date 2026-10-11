using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.DPR;

public class AssignTaxDocumentDataResponse
{
	public List<TaxDataUpdateResult> ImportedData { get; set; }

	public List<Guid> NotFoundISNs { get; set; }

	public List<string> Exceptions { get; set; }
}
