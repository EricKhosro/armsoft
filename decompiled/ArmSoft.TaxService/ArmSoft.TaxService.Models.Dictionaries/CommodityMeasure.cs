using System;

namespace ArmSoft.TaxService.Models.Dictionaries;

public class CommodityMeasure
{
	public Guid Id { get; set; }

	public int OriginalId { get; set; }

	public string Code { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public string NameRu { get; set; } = string.Empty;
}
