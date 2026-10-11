namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class GarbageCollectionRealitiesCriteria
{
	public string Id { get; set; }

	public string PassportNumber { get; set; }

	public string SocialCard { get; set; }

	public string TaxCode { get; set; }

	public GarbageCollectionRealitiesCriteria()
	{
		Id = string.Empty;
		PassportNumber = string.Empty;
		SocialCard = string.Empty;
		TaxCode = string.Empty;
	}
}
