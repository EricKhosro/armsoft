using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class TP : BP
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<TaxDebt> Rows { get; set; }

	public string TaxPropertyId { get; set; }

	public TaxPropertyType TaxPropertyType { get; set; }

	public string TaxPayerName { get; set; }

	public string TaxPayerAddress { get; set; }
}
