using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.DPR;

public class ChangeTaxInvoiceFieldsRequestModel
{
	public List<string> ExportedInvoicesWithRowISNDict { get; set; }

	public List<Guid> Corrections { get; set; }

	public string ExportedType { get; set; }

	public string LogComment { get; set; }
}
