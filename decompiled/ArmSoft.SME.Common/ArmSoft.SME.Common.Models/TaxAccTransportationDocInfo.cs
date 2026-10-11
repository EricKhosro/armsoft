using System;

namespace ArmSoft.SME.Common.Models;

public class TaxAccTransportationDocInfo
{
	public Guid? ISN { get; set; }

	public string TaxDocInnerType { get; set; } = "";

	public string DocNumber { get; set; } = "";

	public string TaxSerialNumber { get; set; } = "";

	public DateOnly? SupplyDate { get; set; }

	public DateOnly? SubmitDate { get; set; }

	public string SupplyLocation { get; set; } = "";

	public string ConveyLocation { get; set; } = "";

	public string SupplierTin { get; set; } = "";

	public decimal TotalQuantity { get; set; }

	public int RowsCount { get; set; }

	public string FileName { get; set; } = "";
}
