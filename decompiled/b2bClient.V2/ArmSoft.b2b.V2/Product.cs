using System;
using System.Collections.Generic;

namespace ArmSoft.b2b.V2;

public class Product
{
	public int Id { get; set; }

	public IdName Actor { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public string FullName { get; set; }

	public List<Unit> Units { get; set; }

	public bool IsService { get; set; }

	public bool HasAttachment { get; set; }

	public Guid Guid { get; set; }
}
