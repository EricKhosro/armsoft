using System;

namespace ArmSoft.SME.Common.Models.ASReport;

public class AccDoc
{
	public DateTime DocDate { get; set; }

	public DateOnly? SubmissionDate { get; set; }

	public string TaxNumber { get; set; }

	public string TaxSeries { get; set; }

	public int RowCount { get; set; }

	public decimal TotalQuantity { get; set; }

	public string AdditionalData { get; set; }
}
