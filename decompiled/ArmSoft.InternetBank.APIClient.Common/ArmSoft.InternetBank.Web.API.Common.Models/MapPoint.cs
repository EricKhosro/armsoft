using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class MapPoint
{
	public int Id { get; set; }

	public byte TypeCode { get; set; }

	public string Name { get; set; }

	public string Address { get; set; }

	public string PhoneNumber { get; set; }

	public string Description { get; set; }

	public decimal Latitude { get; set; }

	public decimal Longitude { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<MapPointWorkingSchedule> MapPointWorkingSchedules { get; set; }
}
