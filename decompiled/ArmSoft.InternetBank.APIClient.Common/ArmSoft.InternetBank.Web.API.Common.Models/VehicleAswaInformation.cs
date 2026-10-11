using System;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class VehicleAswaInformation
{
	public string Mark { get; set; }

	public string Model { get; set; }

	public string HorsePower { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime SuggestedStartDate { get; set; }

	public string OwnerName { get; set; }

	public string OwnerIdNumber { get; set; }

	public short OwnerBonusMalus { get; set; }

	public ClientStatus PolicyHolderStatus { get; set; }

	public string Token { get; set; }
}
