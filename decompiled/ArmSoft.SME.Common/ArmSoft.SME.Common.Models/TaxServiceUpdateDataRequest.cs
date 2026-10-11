using System;

namespace ArmSoft.SME.Common.Models;

public class TaxServiceUpdateDataRequest
{
	public Guid ISN { get; set; }

	public string TaxSerialNumber { get; set; }

	public DateOnly? SubmitDate { get; set; }

	public decimal Total { get; set; }

	public bool AssignTaxNumToDocNum { get; set; }

	public short DocType { get; set; }
}
