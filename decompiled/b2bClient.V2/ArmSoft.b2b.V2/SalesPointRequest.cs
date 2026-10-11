using System.Collections.Generic;

namespace ArmSoft.b2b.V2;

public class SalesPointRequest
{
	public string TaxCode { get; set; }

	public string Name { get; set; }

	public int Spheres { get; set; }

	public AddressRequest Address { get; set; }

	public bool IncludeClosed { get; set; }

	public bool IncludeAddress { get; set; }

	public bool IncludeContacts { get; set; }

	public uint? MaxLimit { get; set; }

	public FieldsFilter<SalesPoint> FieldsFilter { private get; set; }

	public List<string> Fields => FieldsFilter?.GetFieldNames();
}
