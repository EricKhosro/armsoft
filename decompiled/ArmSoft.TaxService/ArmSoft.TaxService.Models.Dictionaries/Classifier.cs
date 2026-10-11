using System;

namespace ArmSoft.TaxService.Models.Dictionaries;

public class Classifier
{
	public Guid Id { get; set; }

	public string Code { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public bool Visibility { get; set; }
}
