namespace ArmSoft.b2b.V2;

public class OrganizationStatistics
{
	public string TaxCode { get; set; }

	public string FullName { get; set; }

	public int StateExtended { get; set; }

	public bool IsVATPayer { get; set; }

	public int TaxationMode { get; set; }

	public int ActorsCount { get; set; }

	public int UsersCount { get; set; }

	public int ProductsCount { get; set; }
}
