using System;

namespace ArmSoft.TaxService.Models;

public class ItemRemainder
{
	public Guid Id { get; set; }

	public decimal Quantity { get; set; }

	public decimal QuantityByFEACN { get; set; }
}
