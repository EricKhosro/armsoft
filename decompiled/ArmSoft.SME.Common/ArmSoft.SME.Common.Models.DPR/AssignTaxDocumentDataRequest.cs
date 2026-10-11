using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.DPR;

public class AssignTaxDocumentDataRequest
{
	public bool CheckByISN { get; set; }

	public string SupplyOrDeliveryAddress { get; set; }

	public short DocType { get; set; }

	public List<TaxDocInfo> TaxDocsInfo { get; set; }
}
