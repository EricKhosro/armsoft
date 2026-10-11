using System;

namespace ArmSoft.b2b.V2;

public class Actor
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string TaxCode { get; set; }

	public DateTime CreationDate { get; set; }

	public DateTime? LastUpdateDate { get; set; }
}
