namespace ArmSoft.b2b.V2;

public class Address
{
	public int Id { get; set; }

	public string Country { get; set; }

	public string Region { get; set; }

	public string City { get; set; }

	public string District { get; set; }

	public string Street { get; set; }

	public string House { get; set; }

	public string Apartment { get; set; }

	public string PostCode { get; set; }

	public bool IsCity { get; set; }

	public GeoLocation Location { get; set; }

	public string FullPath { get; set; }
}
