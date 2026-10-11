using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class BasePropertyTaxPayment : BP
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<PropertyTaxDebt> Rows { get; set; }

	public string PropertyTaxDebtPeriod { get; set; }

	public string PropertyTaxOwnerLegalStatus { get; set; }

	public string PropertyTaxOwnerTaxCode { get; set; }

	public string PropertyTaxOwnerSocialCard { get; set; }

	public string PropertyTaxCode { get; set; }

	public PropertySearchMethod PropertySearchMethod { get; set; }

	public string PropertyType { get; set; }
}
