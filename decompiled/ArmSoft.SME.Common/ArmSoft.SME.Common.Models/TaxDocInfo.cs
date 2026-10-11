using System;

namespace ArmSoft.SME.Common.Models;

public class TaxDocInfo
{
	public Guid? ISN { get; set; }

	public string TaxDocInnerType { get; set; } = "";

	public string DocNumber { get; set; } = "";

	public string TaxSerialNumber { get; set; } = "";

	public string ParentSerialNumber { get; set; } = "";

	public DateOnly? SupplyDate { get; set; }

	public DateOnly? SubmitDate { get; set; }

	public DateOnly? CoSignDate { get; set; }

	public string BusinessAddress { get; set; } = "";

	public string SupplierTin { get; set; } = "";

	public string BuyerTinOrPassport { get; set; } = "";

	public decimal TotalSum { get; set; }

	public string FileName { get; set; } = "";
}
