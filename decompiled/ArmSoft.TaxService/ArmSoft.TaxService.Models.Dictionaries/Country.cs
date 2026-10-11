using System;

namespace ArmSoft.TaxService.Models.Dictionaries;

public class Country
{
	public Guid Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public string Code { get; set; } = string.Empty;
}
