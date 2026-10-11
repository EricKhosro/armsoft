namespace ArmSoft.b2b.V2;

public class OrganizationRequest
{
	public string Name { get; set; }

	public string TaxCode { get; set; }

	public bool IncludeAddresses { get; set; }

	public uint? MaxLimit { get; set; }
}
