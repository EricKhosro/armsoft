using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class TaxProperty
{
	public string Id { get; set; }

	public TaxPropertyType Type { get; set; }

	public string Info { get; set; }

	public string Name { get; set; }

	public string SocialCard { get; set; }

	public string TaxCode { get; set; }
}
