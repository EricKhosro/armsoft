using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.DPR;

public class AssignTaxAccTransportationDocumentDataRequest
{
	public bool CheckByISN { get; set; }

	public short DocType { get; set; }

	public List<TaxAccTransportationDocInfo> TaxDocsInfo { get; set; }
}
