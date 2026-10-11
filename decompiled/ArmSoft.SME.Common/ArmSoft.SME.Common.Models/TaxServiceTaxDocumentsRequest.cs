using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Common.Models;

public class TaxServiceTaxDocumentsRequest
{
	public List<TaxDocument> TaxDocuments { get; set; }

	public DateTime MinDate { get; set; }

	public DateTime MaxDate { get; set; }

	public List<short> DocumentTypes { get; set; }

	public bool AssignTaxInvNumToDocNum { get; set; }
}
