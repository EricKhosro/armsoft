using System;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common;

public class AswaGetInsuranceOffersParams
{
	public string Token;

	public int VehicleUseTypeId { get; set; }

	public string PhoneNumber { get; set; }

	public string Email { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime StartDate { get; set; }

	public int DurationInMonths { get; set; }

	public string DefaultAccount { get; set; }
}
