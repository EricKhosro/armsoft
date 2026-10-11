using System;

namespace ArmSoft.TaxService.Models;

public class Signature
{
	public Guid Id { get; set; }

	public string DocumentHash { get; set; } = string.Empty;

	public string SupplierSignature { get; set; } = string.Empty;

	public string SupplierCertificate { get; set; } = string.Empty;

	public string SupplierCn { get; set; } = string.Empty;

	public DateTime? SupplierSignedAt { get; set; }

	public string BuyerSignature { get; set; } = string.Empty;

	public string BuyerCertificate { get; set; } = string.Empty;

	public string BuyerCn { get; set; } = string.Empty;

	public DateTime? BuyerSignedAt { get; set; }
}
