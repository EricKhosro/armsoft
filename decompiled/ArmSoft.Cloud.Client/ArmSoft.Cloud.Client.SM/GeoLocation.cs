using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.SM;

public class GeoLocation
{
	[JsonProperty("latitude")]
	public double Latitude { get; set; }

	[JsonProperty("longitude")]
	public double Longitude { get; set; }

	[JsonProperty("accuracy")]
	public double Accuracy { get; set; }
}
