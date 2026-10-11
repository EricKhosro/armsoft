using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class GarbageCollectionTaxDebtRequestParams
{
	public string PropertyTaxOwnerSocialCard { get; set; }

	public string PropertyTaxOwnerTaxCode { get; set; }

	public string PropertyTaxOwnerLegalStatus { get; set; }

	public string PropertyTaxDebtPeriod { get; set; }

	public string PropertyTaxCode { get; set; }

	public PropertySearchMethod PropertySearchMethod { get; set; }
}
