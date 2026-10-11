using System;

namespace ArmSoft.SME.Common.Models.DPR;

public class TaxAccTransportationDataUpdateResult
{
	public Guid ISN { get; set; }

	public bool IsMapped { get; set; }

	public bool IsMappedByISN { get; set; }

	public string NotMappingReason { get; set; } = "";

	public string DocNumbers { get; set; } = "";

	public TaxAccTransportationDocInfo TaxDocsInfo { get; set; }
}
